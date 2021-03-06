﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerInit : NetworkBehaviour
{
    [SerializeField] private GameObject m_camera;
    [SerializeField] private GameObject m_Mesh;
    void Start()
    {
        if (isLocalPlayer)
        {
            m_camera.SetActive(true);
            //Disable the objects storing the red/blue meshes because m_Mesh is used to compute the tilt from the movements)
            IsMeshEnabled(false);
        }
        else
        {
            DisableControls();
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void IsMeshEnabled(bool b)
    {
        foreach (Transform child in m_Mesh.transform)
                child.gameObject.SetActive(b);
    }
    void DisableControls()
    {
        m_camera.SetActive(false);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<ShootCommand>().enabled = false;
    }

    void CutNetworkTransform()
    {
        GetComponent<NetworkTransform>().enabled = false;
    }

    void Dollify(Vector3 v)
    {
        DisableControls();
        CutNetworkTransform();
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = v;
        r.isKinematic = false;
        r.freezeRotation = false;
        if(isLocalPlayer)
            IsMeshEnabled(true);
    }
    [ClientRpc]
    void RpcDollify(Vector3 v)
    {
        Dollify(v);
    }

    [Command]
    public void CmdDollify()
    {
        RpcDollify(GetComponent<Rigidbody>().velocity);
    }
}
