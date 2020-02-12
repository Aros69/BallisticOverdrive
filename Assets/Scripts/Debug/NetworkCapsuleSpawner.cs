using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkCapsuleSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_toSpawnObject;
    // Start is called before the first frame update
    void Start() {
        
    }

    private void doTheAction(){
        CmdSpawnCapsule();
    }

    [Command]
    private void CmdSpawnCapsule(){
        GameObject projectile = Instantiate(m_toSpawnObject, new Vector3(), Quaternion.identity);
        NetworkServer.Spawn(projectile, base.connectionToClient);
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer && Input.GetButtonDown("Fire1")){
            doTheAction();
        }
    }
}
