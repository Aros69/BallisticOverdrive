using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Projectile : NetworkBehaviour
{
    private Rigidbody m_rigidBody;
    private GameObject m_owner;

    [SerializeField] private float m_speed = 20.0f;

    [SerializeField] GameObject m_explosionRed = null;
    [SerializeField] GameObject m_explosionBlue = null;
    [SerializeField] Transform m_explosionPoint = null;
    /*private Team m_team;
    public Team team {
        get => m_team;
        set { m_team = value; }
    }*/

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        //m_team = GetComponent<TeamManager>().getTeam();
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

	public void SetOwner(NetworkIdentity conn)
    {
		//Debug.Log("set owner");
		if (ClientScene.localPlayer.GetComponent<NetworkIdentity>().netId == conn.netId 
			&& m_owner.GetComponent<NetworkIdentity>().hasAuthority // implicite
			&& !hasAuthority)
		{
			//Debug.Log("no authority ask for");
			CmdRequestAuthority(GetComponent<NetworkIdentity>());
		}

	}
    
    void Explode(Vector3 explosionPoint)
    {
        //Debug.Log(GetComponent<TeamManager>().getTeam());
        if (GetComponent<TeamManager>().getTeam() == Team.Red)
        {
            Instantiate(m_explosionRed, explosionPoint, transform.rotation);
        } else
        {
            Instantiate(m_explosionBlue, explosionPoint, transform.rotation);
        }
		if (gameObject.transform.parent != null)

			Destroy(gameObject.transform.parent);
		else
			Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
		//Debug.Log("collide");
		if (hasAuthority)
		{
			//Debug.Log("has authority");
            if((col.gameObject.GetComponent<NetworkIdentity>() != null && !col.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            || col.gameObject.GetComponent<NetworkIdentity>() == null) // Only collide with others
            {
				//Debug.Log("Bullet collide something");
				if (col.gameObject.layer == LayerMask.NameToLayer("Hitable"))
                    {
                        
                        if(!col.GetComponent<TeamManager>().isProjectile() && !col.GetComponent<TeamManager>().getTeam().Equals(GetComponent<TeamManager>().getTeam()))
						{
							//Debug.Log("Collide player");

							col.GetComponent<Hit>().hit(gameObject);
							ClientScene.localPlayer.GetComponent<ServerCommunication>().CmdPlayerHit(col.gameObject);
						}

					}
                if (m_explosionPoint == null)
                {
                    Explode(transform.position);
                    CmdExplodeToOther(transform.position);
                }
                else
                {
                    Explode(m_explosionPoint.position);
                    CmdExplodeToOther(m_explosionPoint.position);
                }
            }
        }

        if ((col.gameObject.GetComponent<NetworkIdentity>() != null && !col.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            || col.gameObject.GetComponent<NetworkIdentity>() == null) // Only collide with others
        {
        
            }
    }

    [Command]
    public void CmdExplodeToOther(Vector3 explosionPosition)
    {
        //Debug.Log("On lance la commande d'explosion pour les autres clients.");
        Explode(explosionPosition);
        RpcExplode(explosionPosition);
    }

    [ClientRpc]
    public void RpcExplode(Vector3 explosionPosition)
    {
        //Debug.Log("On fait exploser la bullet chez les autres clients.");
        Explode(explosionPosition);
    }

    [Command]
	private void CmdRequestAuthority(NetworkIdentity otherId)
	{
		//Debug.Log("Request authority");
		otherId.AssignClientAuthority(base.connectionToClient);
	}
}
