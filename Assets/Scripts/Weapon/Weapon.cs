using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_player = null;
    private ShootCommand m_playerShootScript = null;

    [SerializeField] protected GameObject m_projectile = null;
    public GameObject projectile {
        get => m_projectile;
        set { m_projectile = value; }
    }
    [SerializeField] protected Transform m_shootSpawn = null;
    public Transform shootSpawn
    {
        get => m_shootSpawn;
    }

    [SerializeField] protected float m_maxCapacity = 3;
    public float maxCapacity
    {
        get => m_maxCapacity;
    }
    [SerializeField] protected float m_reloadTime = 1;
    [SerializeField] protected float m_delay = 0.5f;
    public float delay { get => m_delay; }

    private float m_ammo = 3;
    public float ammo { get => m_ammo; }

    //Automatic m_reloading
    private float m_reloading = 0;
    public float reloading { get => m_reloading; }
    private float m_startedReload = 0;

    // return false if unable to shoot
    public bool Shoot()
    {
        if (m_ammo == 0)
            return false;
        else
        {
            //CmdShoot();
            //GameObject bullet = Instantiate(m_projectile, m_shootSpawn.position, m_shootSpawn.rotation);
            m_playerShootScript.shoot();
            m_ammo--;
            //Debug.Log("Shot after server command");
        }
        return true;
    }
    /*
    // Impossible to use because Weapon Object is not a client
    [Command]
    public void CmdShoot()
    {
        Debug.Log("Shot from server command");
        GameObject bullet = Instantiate(m_projectile, m_shootSpawn.position, m_shootSpawn.rotation);
        NetworkServer.Spawn(bullet, base.connectionToClient);
        bullet.GetComponent<Projectile>().SetOwner(gameObject);
    }
    */

    public void UpdateAmmo()
    {

        if (m_ammo < m_maxCapacity)
        {
            // We want to keep track of when we started m_reloading current bullet, set to time if not already the case
            if (m_startedReload == 0)
                m_startedReload = Time.time;

            m_reloading = (Time.time - m_startedReload) / m_reloadTime - m_delay;
            if (m_reloading > 1.0)
            {
                m_ammo++;
                m_startedReload = 0;
                m_reloading = 0;
            }
        }
    }
    
    void Awake()
    {
        m_ammo = m_maxCapacity;
        m_shootSpawn = transform.GetChild(1);
        m_playerShootScript = m_player.GetComponent<ShootCommand>();
    }

    // Update is called once per frame
    void Update()
    { }  
}
