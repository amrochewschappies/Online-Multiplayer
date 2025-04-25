using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class LobbyUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button joinButton;
    public GameObject hostPanel;  // Add reference to the host panel

    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            PlayerNameHolder.playerName = nameInput.text;
            NetworkManager.singleton.StartHost();

            // Show host panel after starting the host
            LobbyUIManager.Instance.ShowHostPanel();
        });

        joinButton.onClick.AddListener(() =>
        {
            PlayerNameHolder.playerName = nameInput.text;
            NetworkManager.singleton.networkAddress = ipInput.text;
            NetworkManager.singleton.StartClient();
        });
    }
}