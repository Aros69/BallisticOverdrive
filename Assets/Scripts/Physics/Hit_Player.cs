﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Player : Hit
{
    override public void hit(GameObject o)
    {
        Debug.Log("Player got hit !");
        Vector3 f = o.GetComponent<Projectile>().getDirection() * o.GetComponent<Rigidbody>().mass;
        GetComponent<Rigidbody>().AddForce(f);
        GetComponent<PlayerSounds>().PlayOuchSound();
        if(GetComponent<HealthManager>().takeDamage())
            Destroy(gameObject);
    }
}
