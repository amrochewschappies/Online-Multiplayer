using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;
public class LobbyUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button joinButton;


    
    
    //ui custom lobby display logic
    [Header("UI Custom Lobby Display Logic")]
    public Button  CustomHostButton;
    public Button  CustomJoinButton;
    public Button TutButton;
    public GameObject hostPanel;
    public GameObject JoinLobbyPanel;
    public GameObject CustomPanel;
    [SerializeField] private bool nameAdded;
    public GameObject ErrorText;
    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            StartCoroutine(loadLobby());
        });

        joinButton.onClick.AddListener(() =>
        {
            PlayerNameHolder.playerName = nameInput.text;
            NetworkManager.singleton.networkAddress = ipInput.text;
            NetworkManager.singleton.StartClient();
        });
        
        CustomHostButton.onClick.AddListener(() =>
        {
            PlayerNameHolder.playerName = nameInput.text;
            
            if (IsNameValid())
            {   
                hostPanel.SetActive(true);
                CustomPanel.SetActive(false);
                JoinLobbyPanel.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowErrorText());
            }
        });
        
        CustomJoinButton.onClick.AddListener(() =>
        {
            PlayerNameHolder.playerName = nameInput.text;
            if (IsNameValid())
            {
                JoinLobbyPanel.SetActive(true);    
                CustomPanel.SetActive(false);
                hostPanel.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowErrorText());
                
            }
        });
        TutButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TutorialScene");
        });
    }
    private void Update()
    {
        if (nameInput.text != null)
        {
            nameAdded = true;
        }
        else if (nameInput.text == null)
        {
            nameAdded = false;
        }
        
    }
    private bool IsNameValid()
    {
        return !string.IsNullOrWhiteSpace(nameInput.text);
    }
    IEnumerator ShowErrorText()
    {
        ErrorText.SetActive(true);
        yield return new WaitForSeconds(2f);
        ErrorText.SetActive(false);
    }

    IEnumerator loadLobby()
    {
        NetworkManager.singleton.StartHost();
        Debug.LogWarning("Server has started.");
        yield return new WaitForSeconds(0.01f);
        NetworkManager.singleton.ServerChangeScene("LobbyScene");
        Debug.LogWarning("LobbyScene has been set.");
    }
}