using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Beam : NetworkBehaviour
{

    void playSound(){
        GetComponent<AudioSource>().Play(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        playSound();
    }

    public void shoot(){
        // Launch a hit scan from the middle of the viewport in front of him. 
        // If the hitscan touch an object, call the hit function of this object
            // and define the length of the laserbeam based on the distance of the object
        // Else use a base distance (100 or less... I don't know)
        // Show for a certain time (less than a sec... 0,5s maybe) the shot base on
        // the distance and the start shot point
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
