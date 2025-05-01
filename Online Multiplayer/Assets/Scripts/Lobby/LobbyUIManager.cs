using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    public Transform playerListParent;
    public GameObject playerItemPrefab;
    public GameObject hostPanel;  // Add reference to the host panel
    public Button startGameButton;
    

    void Awake()
    {
        Instance = this;
    }

    public void UpdatePlayerList()
    {
        if (NetworkServer.connections.Count > 1)
        {
            startGameButton.interactable = true;
        }
        else
        {
            startGameButton.interactable = false;
        }
    
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<LobbyPlayer>();
                if (player != null)
                {
                    GameObject item = Instantiate(playerItemPrefab, playerListParent);
                    item.GetComponentInChildren<Text>().text = player.playerName;
                }
            }
        }
    }
    
}