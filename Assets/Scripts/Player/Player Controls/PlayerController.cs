using System.Collections;
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
    [Header("The 3D model of the player")]
    [SerializeField]
    public GameObject m_Mesh;
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
    private float m_tiltStrenght = 200.0f;
    [SerializeField]
    private float m_lookSensitivity = 3.0f;
    [SerializeField]
    private float m_viewRange = 60.0f;
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

	private void Start()
	{
        m_rigidBody = GetComponent<Rigidbody>();

	}
	

    public void BlockMovement()
    {
        // m_rigidBody.useGravity = false;
        // enabled = false;
    }
    public void Teleport(Vector3 v)
    {
        if(isLocalPlayer)
            m_rigidBody.position = v;
    }
	// Use this for initialization
	public override void OnStartLocalPlayer()
    {
        m_Camera.gameObject.SetActive(true);
        m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
        Cursor.visible = false;
        m_height = transform.localScale.y;
        m_width = transform.localScale.x;
        m_collider = GetComponent<CapsuleCollider>();
		//CmdSpawnGameManager();
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
        m_velocity += kinecticForce * Time.fixedDeltaTime;


        //Compute camera rotation
        m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity;
        m_cameraRotation = new Vector3(m_xRot, 0, 0) * m_lookSensitivity;
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && m_grounded)
        {
            GetComponent<PlayerSounds>().PlayJumpSound();
            // m_rigidBody.AddForce(Vector3.up * m_jumpforce);
            m_velocity += Vector3.up * m_jumpforce;
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

    /// <summary>
    /// Tilt the player depending on the velocity
    /// </summary>
    void Tilt()
    {
        // m_Mesh.transform.RotateAround(m_Mesh.transform.position, axis, m_velocity.magnitude);
        float forwardTilt = -Vector3.Dot(m_velocity, transform.forward);
        float sidewardTilt = Vector3.Dot(m_velocity, transform.right);
        m_Mesh.transform.localRotation = Quaternion.Euler(  forwardTilt*m_tiltStrenght, 
                                                            m_Mesh.transform.localRotation.eulerAngles.y,
                                                            sidewardTilt*m_tiltStrenght);
    }
    
    /// <summary>
    /// Correct Camera rotations
    /// </summary>
    void CorrectCameraRotation()
    {
        float angle = m_Camera.transform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        m_Camera.transform.eulerAngles = new Vector3( Mathf.Clamp( angle,
             -m_viewRange, m_viewRange), m_Camera.transform.eulerAngles.y, m_Camera.transform.eulerAngles.z);
    }
    public void Update()
    {
        if(isLocalPlayer)
        {
            Jump();
            Crouch();
        }
    }

    public void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            ComputeMovements();
            Tilt();
            m_velocity += new Vector3(0, - m_bonusGravity - 9.8f, 0) * Time.fixedDeltaTime;
            m_rigidBody.velocity = m_velocity;
            Debug.Log(m_velocity);
            m_rigidBody.MoveRotation(m_rigidBody.rotation * Quaternion.Euler(m_rotation));

            if(m_grounded)
                m_velocity *= 1.0f/m_groundDrag;
            else
                m_velocity *= 1.0f/m_airDrag;
            m_Camera.transform.Rotate(-m_cameraRotation);
            CorrectCameraRotation();
        }
    }
}