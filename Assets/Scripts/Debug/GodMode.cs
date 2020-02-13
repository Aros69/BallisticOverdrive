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
    private bool m_isInGodMode = false;

    public bool isInGodMode
    {
        get => m_isInGodMode;
        set { m_isInGodMode = value; }
    }



    [SerializeField]
    private GameObject m_playersPrefab;
    [SerializeField]
    private GameObject m_redShotPrefab;
    [SerializeField]
    private GameObject m_blueShotPrefab;

    private LineRenderer m_aimHelper;
    public LineRenderer aimHelper
    {
        get => m_aimHelper;
        set { m_aimHelper = value; }
    }
    private Camera m_cam;
    public Camera cam
    {
        get => m_cam;
        set { m_cam = value; }
    }

    // Start is called before the first frame update
    void Start() {  
        cam = GetComponent<PlayerController>().m_Camera;
    }

    private void addFakePlayer(Vector3 position){
        GameObject obj = Instantiate(m_playersPrefab, position + new Vector3(0f, 1f, 0f), transform.rotation);
        obj.name = "FakePlayer";
        obj.GetComponent<PlayerController>().enabled = false;
        obj.GetComponent<AmmoManager>().enabled = false;
        obj.GetComponent<GodMode>().isInGodMode = false;
        obj.transform.GetChild(0).GetComponent<Camera>().enabled = false;
        obj.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
        CmdSpawnObject(obj);
    }

    [Command]
    private void CmdSpawnObject(GameObject obj){
        NetworkServer.Spawn(obj);
    }

    // Update is called once per frame
    void Update() {
        if (base.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                isInGodMode = !isInGodMode;
                if (isInGodMode)
                {
                    //Debug.Log("I'm god now");
                    aimHelper = gameObject.AddComponent<LineRenderer>();
                    aimHelper.startWidth = 0.1f;
                    aimHelper.endWidth = 0.1f;
                }
                else
                {
                    //Debug.Log("I'm NOT god anymore");
                    Destroy(aimHelper);
                }
            }
            if (isInGodMode)
            {
                aimHelper.SetPosition(0, transform.position);
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                bool isAimingGround = Physics.Raycast(transform.position, ray.direction, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
                if (isAimingGround)
                {
                    aimHelper.SetPosition(1, ray.GetPoint(hit.distance));
                }
                else
                {
                    aimHelper.SetPosition(1, ray.GetPoint(10));
                }
                if (Input.GetKeyDown(KeyCode.H) && isAimingGround)
                {
                    addFakePlayer(ray.GetPoint(hit.distance));
                }
            }
        }
        
    }
}