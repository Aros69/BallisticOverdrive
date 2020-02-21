using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootCommand : NetworkBehaviour
{
	[SerializeField] private Camera m_camera;
    private Weapon m_weapon;

	public void setWeapon(Weapon weapon)
	{
		m_weapon = weapon;
	}

    public void shoot()
    {
        Ray ray = m_camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        int layerMask = 1 << 9; // Ground layer
        layerMask += 1 << 10; // Hitable layer

        bool niceShot = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
        float distanceHit;
        // If the hitscan touch an object, call the hit function of this object
        // and define the length of the laserbeam based on the distance of the object
        if (niceShot)
        {
            distanceHit = hit.distance;
        }
        // Else use a base distance (100 or less... I don't know)
        else
        {
            distanceHit = 100f;
        }

		

		Vector3 shotDirection = ray.GetPoint(distanceHit) - m_weapon.shootSpawn.position;
		// mega bullet
		if (m_weapon.projectile.GetComponentInChildren<TeamManagerMegaBullet>() != null)
		{
			GameObject megaBullet = m_weapon.projectile;
			for (int i = 0; i < megaBullet.transform.childCount; i++)
			{
				Vector3 pos = m_weapon.shootSpawn.position + megaBullet.transform.GetChild(i).localPosition;
				Quaternion rot = Quaternion.LookRotation(shotDirection, m_weapon.shootSpawn.up) * megaBullet.transform.GetChild(i).localRotation;
				CmdShoot(pos, rot);
			}
		} else
		{
			CmdShoot(m_weapon.shootSpawn.position, Quaternion.LookRotation(shotDirection, m_weapon.shootSpawn.up));
		}



	}


	[Command]
	public void CmdShoot(Vector3 pos, Quaternion rot)
	{
		GameObject bullet;
		if (m_weapon.projectile.GetComponentInChildren<TeamManagerMegaBullet>() != null)
		{
			bullet = Instantiate(GetComponent<WeaponManager>().WeaponSimple.GetComponent<Weapon>().projectile, pos, rot);
		} else
		{
			bullet = Instantiate(m_weapon.projectile, pos, rot);
		}
		bullet.GetComponent<TeamManager>().setTeam(GetComponent<TeamManager>().getTeam());
		NetworkServer.Spawn(bullet, base.connectionToClient);
	}
}
