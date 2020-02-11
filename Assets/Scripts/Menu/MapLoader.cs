using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private string m_mapToLoad = "elite_base";
    // Start is called before the first frame update
    void Start()
    {
        LoadMap(m_mapToLoad);
    }
    
    void LoadMap(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }
}
