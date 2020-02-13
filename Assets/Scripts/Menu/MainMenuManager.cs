using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverAddressInputField;

    void Start(){
        Cursor.visible = true;
    }

    public void ConnectToServer()
    {
        ODNetworkManager.singleton.networkAddress = serverAddressInputField.text;
        ODNetworkManager.singleton.StartClient();
    }

    public void CreateGame()
    {
        ODNetworkManager.singleton.StartHost();
    }

    public void Quit(){
        Application.Quit();
    }
}
