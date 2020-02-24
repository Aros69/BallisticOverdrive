using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerGhostController : MonoBehaviour
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

    [Header("The Camera the player looks through")]
    [SerializeField]
    public Camera m_Camera;

    [Header("Player Movement properties")]
    [SerializeField]
    private float m_speed = 1.0f;
    [SerializeField]
    private float m_airdrag = 1.2f;
    [SerializeField]
    private float m_lookSensitivity = 3.0f;
    [SerializeField]
    private float m_viewRange = 60.0f;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        // If the player is not local disable the rigidbody (To prevent gravity being applied two times)
        // if(!isLocalPlayer)
        // BlockMovement();
        //Ghostify();
    }

    public void Ghostify()
    {
        // Disable the capsule collider
        GetComponent<CapsuleCollider>().isTrigger = true;
        m_rigidBody.isKinematic = true;
        //Disable the weapons, which are the children of the camera
        foreach (Transform child in m_Camera.transform)
            child.gameObject.SetActive(false);

        GetComponent<HealthManager>().enabled = false;
        GetComponent<AmmoManager>().enabled = false;
    }

    void ComputeMovements()
    {
        m_MovX = Input.GetAxis("Horizontal");
        m_MovY = Input.GetAxis("Vertical");

        //Get mouse Input
        m_yRot = Input.GetAxisRaw("Mouse X");
        m_xRot = Input.GetAxisRaw("Mouse Y");

        //Compute player speed and velocity
        m_moveHorizontal = m_Camera.transform.right * m_MovX;
        m_moveVertical = m_Camera.transform.forward * m_MovY;

        Vector3 kinecticForce = (m_moveHorizontal + m_moveVertical) * m_speed;

        m_velocity += kinecticForce * Time.fixedDeltaTime;

        //Compute camera rotation
        m_rotation = new Vector3(0, m_yRot, 0) * m_lookSensitivity;
        m_cameraRotation = new Vector3(m_xRot, -m_yRot, 0) * m_lookSensitivity;
    }

    /// <summary>
    /// Correct Camera rotations
    /// </summary>
    void CorrectCameraRotation()
    {
        float angle = m_Camera.transform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        m_Camera.transform.eulerAngles = new Vector3(Mathf.Clamp(angle,
             -m_viewRange, m_viewRange), m_Camera.transform.eulerAngles.y, m_Camera.transform.eulerAngles.z);
    }

    public void FixedUpdate()
    {
        ComputeMovements();

        m_rigidBody.MovePosition(m_rigidBody.position + m_velocity);
        m_Camera.transform.Rotate(-m_cameraRotation);
        m_velocity *= 1.0f/m_airdrag;
        // CorrectCameraRotation();
    }
}
