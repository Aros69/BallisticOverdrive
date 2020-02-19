using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Hit_Player : Hit
{
	override public void hit(GameObject projectile)
    {
        Debug.Log("I hit someone !");
        Vector3 f = projectile.GetComponent<Projectile>().getDirection() * projectile.GetComponent<Rigidbody>().mass;
        //GetComponent<Rigidbody>().AddForce(f);
        GetComponent<PlayerSounds>().PlayOuchSound();
	}
}
