using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CustomNetworkManager : NetworkRoomManager
{
    public List<Transform> spawnPoints = new List<Transform>();  // List of spawn points
    public List<GameObject> Players = new List<GameObject>(); // List of players
    public GameObject roomPrefabPlayer;
    private bool gameStarted = false; 
    private int playerAmount = 0; 
    public static event System.Action OnPlayerReady;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (spawnPoints.Count == 0)
        {
            FindSpawnPoints();  // Dynamically find them
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available! Cannot spawn player.");
            return;
        }
        int spawnIndex = NetworkServer.connections.Count - 1;
        Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Count];
        
        GameObject player = Instantiate(roomPrefabPlayer, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        
        Players.Add(player);
        SortPlayers();
    }
    
    private void FindSpawnPoints() //working 
    {
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints.Clear();

        foreach (GameObject spawnObject in spawnObjects)
        {
            spawnPoints.Add(spawnObject.transform);
        }
    }

    private void SortPlayers() //working
    {
        if (Players.Count == 0) return;

        int half = Players.Count / 2;

        for (int i = 0; i < Players.Count; i++)
        {
            if (i < half)
            {
                Players[i].tag = "Team2";
            }
            else
            {
                Players[i].tag = "Team1";
            }

            playerAmount += 1;
        }
    }

    public override void OnServerChangeScene(string newSceneName) //working
    {
        // first change to lobby scene
        base.OnServerChangeScene(newSceneName); 
        if (newSceneName == "LobbyScene")
        {
            FindSpawnPoints();
        }
        // Do the same and find spawn points
        if (newSceneName == "GameScene")
        {
            FindSpawnPoints();
        }
    }
    
    //this will make sure on player ready fires
    public static void InvokePlayerReady()
    {
        OnPlayerReady?.Invoke();
        Debug.Log("A player has readied up.");
    }

    
    // when all players are ready
        /*public override void OnRoomServerPlayersReady()
        {
            Debug.Log("All players are ready — starting game!");
            ServerChangeScene(GameplayScene); // This will transition to your game scene
        }*/
    

}
