using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private GameObject m_camera;
    [SerializeField] private GameObject m_Mesh;
    void Start()
    {
        if(isLocalPlayer)
        {
            m_camera.SetActive(true);
            //Disable the objects storing the red/blue meshes because m_Mesh is used to compute the tilt from the movements)
            foreach(Transform child in m_Mesh.transform)
                child.gameObject.SetActive(false);
            
        }
        else
            m_camera.SetActive(false);
    }   

   
}
