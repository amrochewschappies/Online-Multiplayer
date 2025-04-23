using UnityEngine;
using Mirror;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    public TMP_InputField ipInputField;

    public void HostGame()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.singleton.networkAddress = ipInputField.text;
        NetworkManager.singleton.StartClient();
    }
}