using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Hit : NetworkBehaviour
{
    public virtual void hit(GameObject o)
    {
        Debug.Log("Hit detected ! ");
        Vector3 f = o.GetComponent<Rigidbody>().velocity * o.GetComponent<Rigidbody>().mass;
        gameObject.GetComponent<Rigidbody>().AddForce(f);
    }
}
