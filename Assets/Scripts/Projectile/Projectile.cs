using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Projectile : NetworkBehaviour
{
    private Rigidbody m_rigidBody;
    private GameObject m_owner;
    
    [SerializeField] private float m_speed = 20.0f;

    [SerializeField] GameObject m_explosion = null;
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

    public void SetOwner(GameObject o)
    {
        m_owner = o;
    }
    
    void Explode(Vector3 explosionPoint)
    {
        Instantiate(m_explosion, explosionPoint, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(hasAuthority)
        {
            if(col.gameObject.GetComponent<NetworkIdentity>() != null && !col.gameObject.GetComponent<NetworkIdentity>().hasAuthority) // Only collide with others
            {
                
                    if(col.gameObject.layer == LayerMask.NameToLayer("Hitable"))
                    {
                        
                        if(!col.GetComponent<TeamManager>().isProjectile() && !col.GetComponent<TeamManager>().getTeam().Equals(GetComponent<TeamManager>().getTeam()))
                            col.GetComponent<Hit>().hit(gameObject);
                    }
                
            }
            Explode(transform.position);
        }
    }
}
