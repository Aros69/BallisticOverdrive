using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Beam : NetworkBehaviour
{
    [SerializeField]
    private Camera m_playerCamera;
    public Camera playerCamera
    {
        get => m_playerCamera;
        set { m_playerCamera = value; }
    }

    [SerializeField]
    private float beamLife = 0.5f; // In sec
    private float timer;

    private LineRenderer beamLine;
    private Transform shootSpawn;


    void playSound()
    {
        GetComponent<AudioSource>().Play(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        beamLine = GetComponent<LineRenderer>();
        //m_playerCamera = transform.parent.transform.GetChild(0).gameObject.GetComponent<Camera>();
        Debug.Log(transform.parent.transform.GetChild(0).gameObject.name);
        playSound();
        timer = 0;
    }

    public void shoot()
    {
        // Launch a hit scan from the middle of the viewport in front of him. 
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        int layerMask = 1 << 9; // Ground layer
        layerMask += 1 << 10; // Hitable layer

        bool niceShot = Physics.Raycast(transform.position, ray.direction, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
        float distanceHit;
        //beamLine.SetPosition(0, )
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
        timer = 0;
        // Show for a certain time (less than a sec... 0,5s maybe) the shot base on
        // the distance and the start shot point
        // How to disolve a material
        //https://answers.unity.com/questions/1599068/triggering-timed-dissolve-shader-in-shader-graph-v.html
    }

    // TODO: resize the beamLine in fonction of the time


    // Update is called once per frame
    void Update()
    {
        if (timer != beamLife)
        {
            if (timer < beamLife)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = beamLife;
            }
            beamLine.material.SetFloat("_DisolveValue", 1f - timer / beamLife);
        }
    }
}
