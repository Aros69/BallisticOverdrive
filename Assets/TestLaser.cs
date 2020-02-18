using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaser : MonoBehaviour {
    public GameObject laserBeam;
    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                GameObject beam = Instantiate(laserBeam);
                beam.transform.position = (hit.point + transform.position)/2.0f;
                beam.transform.localScale = new Vector3(0.1f, hit.distance/2.1f, 0.1f);
                beam.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.point - transform.position);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }
}