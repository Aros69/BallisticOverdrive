using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    [SerializeField]
    private float m_speed = 20.0f;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public Vector3 getDirection()
    {
        return transform.forward;
    }
    private void Update()
    {
        //too far from map
        if(transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        m_rigidBody.MovePosition(m_rigidBody.position + getDirection() * m_speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Target Code : To delete ?
        /*if(other.tag != "Player")
        {
            Target target = other.GetComponent<Target>();
            if(target != null)
            {
                Debug.Log("Hit target");
                target.Hit();
            }
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 0.5f);
            Destroy(this);
        }
        else */
        if(other.gameObject.layer == LayerMask.NameToLayer("Hitable"))
        {
            if(!other.GetComponent<TeamManager>().getTeam().Equals(GetComponent<TeamManager>().getTeam()))
                other.GetComponent<Hit>().hit(gameObject);
        }
    }
}
