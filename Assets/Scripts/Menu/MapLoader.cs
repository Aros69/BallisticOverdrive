using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private string m_mapToLoad = "elite_base";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMap());
    }
    
    IEnumerator LoadMap()
    {
        HUDController.instance.BlackScreen(true);
        yield return SceneManager.LoadSceneAsync(m_mapToLoad, LoadSceneMode.Additive);
        HUDController.instance.BlackScreen(false);

        Debug.Log("Map Loaded!");
    
        ServerCommunication[] serverComs = GameObject.FindObjectsOfType<ServerCommunication>();
        
        foreach (ServerCommunication serverCom in serverComs)
        {
            if(serverCom.gameObject.GetComponent<NetworkIdentity>().hasAuthority){
                serverCom.CmdAddPlayer();
            }
        }
    }
}
