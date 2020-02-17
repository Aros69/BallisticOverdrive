using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootCommand : NetworkBehaviour
{

    [SerializeField] private GameObject m_weaponObj;
    private Weapon m_weapon;

    public void shoot()
    {
        CmdShoot(m_weapon.shootSpawn.position, m_weapon.shootSpawn.rotation);
    }


    [Command]
    public void CmdShoot(Vector3 pos, Quaternion rot)
    {
        //GameObject bullet = Instantiate(m_weapon.projectile, m_weapon.shootSpawn.position, m_weapon.shootSpawn.rotation);
        GameObject bullet = Instantiate(m_weapon.projectile, pos, rot);
        NetworkServer.Spawn(bullet, base.connectionToClient);
        bullet.GetComponent<Projectile>().SetOwner(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        m_weapon = m_weaponObj.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
