using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Player : Hit
{
    override public void hit(GameObject o)
    {
        Debug.Log("Ouch ! ");
        Vector3 f = o.GetComponent<Projectile>().getDirection() * o.GetComponent<Rigidbody>().mass;
        gameObject.GetComponent<Rigidbody>().AddForce(f);
        if(gameObject.GetComponent<HealthManager>().takeDamage())
            Destroy(gameObject);
    }
}
