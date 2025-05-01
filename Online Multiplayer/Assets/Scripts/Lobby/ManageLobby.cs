using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ManageLobby : MonoBehaviour
{
    public static ManageLobby Instance;

    [Header("UI")]
    public Transform playerListContainer;
    public GameObject playerListItemPrefab;

    private Dictionary<uint, GameObject> playerListItems = new Dictionary<uint, GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddPlayer(NetworkIdentity playerIdentity, string playerName)
    {
       //GameObject item = Instantiate(playerListItemPrefab, playerListContainer);
        //var listItem = item.GetComponent<LobbyPlayerListItem>();
        //listItem.SetName(playerName);

        //playerListItems[playerIdentity.netId] = item;
        
        
        
        // when player gets added to server - store in list of playersInLObby[]
        //iterate through players in lobby
        //Assigned 3rd player onwards to team 
    }

    public void RemovePlayer(NetworkIdentity playerIdentity)
    {
        if (playerListItems.TryGetValue(playerIdentity.netId, out GameObject item))
        {
            Destroy(item);
            playerListItems.Remove(playerIdentity.netId);
        }
    }
}
