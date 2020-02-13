using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Discovery;
using TMPro;

public class ODNetworkDiscoveryUIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text serverCount;
    [SerializeField] private GameObject template;
    [SerializeField] private GameObject container;
    private float elementHeight;

    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    private NetworkDiscovery networkDiscovery;

    void Start(){
        networkDiscovery = ODNetworkManager.singleton.gameObject.GetComponent<NetworkDiscovery>();
        networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
        elementHeight = ((RectTransform)template.transform).sizeDelta.y;
    }

    public void Refresh(){
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    private void UpdateServerList(){
        for(int i = 1; i < container.transform.childCount; i++){
            Destroy(container.transform.GetChild(i).gameObject);
        }

        ((RectTransform)container.transform).sizeDelta = new Vector2(((RectTransform)container.transform).sizeDelta.x, elementHeight*discoveredServers.Count);

        int count = 0;
        foreach (ServerResponse info in discoveredServers.Values){
            GameObject item = Instantiate(template, container.transform);
            item.SetActive(true);
            ((RectTransform)item.transform).anchoredPosition = new Vector2(0, -elementHeight*count);
            item.transform.GetChild(1).GetComponent<TMP_Text>().text = info.EndPoint.Address.ToString();
            item.GetComponent<Button>().onClick.AddListener(delegate{Connect(info.serverId);});
        }
    }

    void Connect(long serverId)
    {
        NetworkManager.singleton.StartClient(discoveredServers[serverId].uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
        UpdateServerList();
        serverCount.text = "Found " + discoveredServers.Count + " servers";
    }
}
