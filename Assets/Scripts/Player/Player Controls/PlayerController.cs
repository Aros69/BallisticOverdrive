﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerController : NetworkBehaviour
{
    // Input store values
    private float m_MovX;
    private float m_MovY;
    private float m_yRot;
    private float m_xRot;

    // Player measurements
    private float m_height;
    private float m_width;

    // Movement store values
    private Vector3 m_moveHorizontal;
    private Vector3 m_moveVertical;
    private Vector3 m_velocity;
    private Rigidbody m_rigidBody;
    private Vector3 m_rotation;
    private Vector3 m_cameraRotation;
    private CapsuleCollider m_collider;
    
    // Movement state values
    private bool m_grounded;
    
    [Header("The Camera the player looks through")]
    [SerializeField]
    public Camera m_Camera;
    [Header("Position of the camera when the player is standing")]
    [SerializeField]
    public Transform m_standingCameraPosition;
    [Header("Position of the camera when the player is crouching")]
    [SerializeField]
    public Transform m_crouchingCameraPosition;
    [Header("Player Movement properties")]
    [SerializeField]
    private float m_speed = 5.0f;
    [SerializeField]
    private float m_lookSensitivity = 3.0f;
    [SerializeField]
    private float m_jumpforce = 300.0f;

    [SerializeField]
    private float m_airDrag = 1.0f;
    [SerializeField]
    private float m_groundDrag = 1.2f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float m_airControl = 0.05f;
    [SerializeField]
    private float m_bonusGravity = 2.0f;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        m_Camera.gameObject.SetActive(true);
        m_rigidBody = GetComponent<Rigidbody>();
        m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
        Cursor.visible = false;
        m_height = transform.localScale.y;
        m_width = transform.localScale.x;
        m_collider = GetComponent<CapsuleCollider>();
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, m_height+0.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
    }
    void ComputeMovements()
    {
        m_grounded = isGrounded();

        m_MovX = Input.GetAxis("Horizontal");
        m_MovY = Input.GetAxis("Vertical");
            
        //Get mouse Input
        m_yRot = Input.GetAxisRaw("Mouse X");
        m_xRot = Input.GetAxisRaw("Mouse Y");

        //Compute player speed and velocity
        m_moveHorizontal = transform.right * m_MovX;
        m_moveVertical = transform.forward * m_MovY;
        
        Vector3 kinecticForce = (m_moveHorizontal + m_moveVertical)* m_speed;
        if(!m_grounded)
            kinecticForce *= m_airControl;
        m_velocity += kinecticForce;
        // if (m_velocity.magnitude > 1.0f) m_velocity = m_velocity.normalized;

        //Compute camera rotation
        m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity;
        m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_lookSensitivity;
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && m_grounded)
        {
            GetComponent<PlayerSounds>().PlayJumpSound();
            m_rigidBody.AddForce(Vector3.up * m_jumpforce);
        }
    }

    void Crouch()
    {
        if(Input.GetKey(KeyCode.LeftControl) && m_grounded)
        {
            m_collider.height = 1.0f;
            m_collider.center = new Vector3(m_collider.center.x, -0.5f, m_collider.center.z);
            m_Camera.transform.position = m_crouchingCameraPosition.position;
        }
        else
        {
            m_collider.height = 2.0f;
            m_collider.center = new Vector3(m_collider.center.x, 0.0f, m_collider.center.z);
            m_Camera.transform.position = m_standingCameraPosition.position;
        }
    }
    public void Update()
    {
        if(isLocalPlayer)
        {
            ComputeMovements();
            Jump();
            Crouch();
        }
    }

    public void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            m_rigidBody.AddForce(new Vector3(0, - m_bonusGravity, 0), ForceMode.Force);
            if(m_grounded)
                m_velocity *= 1.0f/m_groundDrag;
            else
                m_velocity *= 1.0f/m_airDrag;
            
            m_rigidBody.MovePosition(m_rigidBody.position + m_velocity * Time.fixedDeltaTime);
            m_rigidBody.MoveRotation(m_rigidBody.rotation * Quaternion.Euler(m_rotation));
            m_Camera.transform.Rotate(-m_cameraRotation);
        }
    }

}