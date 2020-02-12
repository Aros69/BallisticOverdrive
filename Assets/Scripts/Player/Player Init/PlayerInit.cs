using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private GameObject m_camera;
    void Start()
    {
        if(isLocalPlayer)
            m_camera.SetActive(true);
        else
            m_camera.SetActive(false);
    }   

   
}
