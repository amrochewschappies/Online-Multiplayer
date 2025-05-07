using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;


public class CustomNetworkManager : NetworkRoomManager
{
    public static CustomNetworkManager instance;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> Players = new List<GameObject>(); 
    public GameObject roomPrefabPlayer;
    private bool gameStarted = false;
    private int playerAmount = 0;
    public static event System.Action OnPlayerReady;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (spawnPoints.Count == 0)
        {
            FindSpawnPoints(); // Dynamically find them
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
            Debug.Log(spawnObject.name);
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

        base.OnServerChangeScene(newSceneName);
        if (newSceneName == "LobbyScene")
        {
            FindSpawnPoints();
        }

        if (newSceneName == "GameScene")
        {
            FindSpawnPoints();
            StartCoroutine(FindSpawnPointsAfterLoad());
            DestroyRoomPlayers();
            GameObject player = GameObject.Find("Player");
            
        }
    }

    private IEnumerator FindSpawnPointsAfterLoad()
    {

        while (!NetworkManager.singleton.isNetworkActive)
        {
            yield return null;
        }


        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints.Clear();

        foreach (GameObject spawnObject in spawnObjects)
        {
            spawnPoints.Add(spawnObject.transform);
        }


        Debug.Log("Spawn points updated in GameScene");
    }

    private void DestroyRoomPlayers()
    {
      
        foreach (GameObject player in Players)
        {
            if (player != null)
            {
                Destroy(player);  
            }
        }
        
        Players.Clear();

        Debug.Log("Room players destroyed and list cleared.");
    }
    //this will make sure on player ready fires
    public static void InvokePlayerReady()
    {
        OnPlayerReady?.Invoke();
        Debug.Log("A player has readied up.");
    }
    
    public override void OnRoomServerPlayersReady()
    {
        int playerCount = roomSlots.Count;
        
        if (playerCount == 1 || playerCount == 3) return;
        
        
        ServerChangeScene(GameplayScene); 
    }
}