using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
 * Put this script on your network player to give 
 * him extra abilities usefull for debugging like 
 * adding player, controlling added players)...
*/
public class GodMode : NetworkBehaviour
{
    private bool isInGodMode = false;

    [SerializeField]
    private GameObject m_playersPrefab;
    [SerializeField]
    private GameObject m_redShotPrefab;
    [SerializeField]
    private GameObject m_blueShotPrefab;

    private void addFakePlayer(){
        //return Physics.Raycast(transform.position, -transform.up, m_height+0.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
        GameObject obj = Instantiate(m_playersPrefab, new Vector3(), Quaternion.identity);
        CmdSpawnObject(obj);
    }

    [Command]
    private void CmdSpawnObject(GameObject obj){
        NetworkServer.Spawn(projectile, base.connectionToClient);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Caret)){
            isInGodMode = !isInGodMode;
        }
        if(isInGodMode){
              if(Input.GetKeyDown(KeyCode.Caret)){
                  addFakePlayer();
              }  
        } 
        
    }
}