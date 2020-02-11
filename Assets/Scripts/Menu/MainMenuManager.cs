using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverAddressInputField;

    public void ConnectToServer()
    {
        ODNetworkManager.singleton.networkAddress = serverAddressInputField.text;
        ODNetworkManager.singleton.StartClient();
    }
}
