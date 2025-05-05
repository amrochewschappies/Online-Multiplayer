using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : NetworkRoomManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")] [SerializeField] private GameNetworkRoomPlayer roomPlayerPrefab = null;
    /*[Header("Game")]
     [SerializeField] private GameNetworkGamePlayer gamePlayerPrefab = null;
     [SerializeField] private GameObject playerSpawnSystem = null;
     [SerializeField] private GameObject roundSystem = null;*/


    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action <NetworkConnection> onServerReadied; 

    public List<GameNetworkRoomPlayer> RoomPlayers { get; } = new List<GameNetworkRoomPlayer>();


    public override void OnStartServer()
    {
        base.OnStartServer(); // Always call base in lifecycle methods

        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();


        GameNetworkRoomPlayer.OnPlayerReady += HandlePlayerReady;
    }

    public override void OnStartClient()
    {
        foreach (var prefab in Resources.LoadAll<GameObject>("SpawnablePrefabs"))
        {
            if (prefab.GetComponent<NetworkIdentity>() != null)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
            else
            {
                Debug.LogWarning($"Prefab '{prefab.name}' has no NetworkIdentity and won't be registered.");
            }
        }


    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
        Debug.Log("A Client Connected to Server.");

    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
           
            bool isLeader = RoomPlayers.Count == 0;

            GameNetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            
           roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<GameNetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
       // OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        //GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }
    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }
    
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart()) { return; }

            // mapHandler = new MapHandler(mapSet, numberOfRounds);

            ServerChangeScene("GameScene");
        }
    }
    
    private void HandlePlayerReady(NetworkConnectionToClient conn)
    {
        Debug.Log($"Player {conn.connectionId} is ready.");
        onServerReadied?.Invoke(conn); 
    }
    
            
}
/*




    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart()) { return; }

           // mapHandler = new MapHandler(mapSet, numberOfRounds);

            //ServerChangeScene(mapHandler.NextMap);
        }
    }

 public void StartGame()
 {
     if (SceneManager.GetActiveScene().name == menuScene)
     {
         if (!IsReadyToStart()) { return; }

         mapHandler = new MapHandler(mapSet, numberOfRounds);

         ServerChangeScene(mapHandler.NextMap);
     }
 }

 public override void ServerChangeScene(string newSceneName)
 {
     // From menu to game
     if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
     {
         for (int i = RoomPlayers.Count - 1; i >= 0; i--)
         {
             var conn = RoomPlayers[i].connectionToClient;
             var gameplayerInstance = Instantiate(gamePlayerPrefab);
             gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

             NetworkServer.Destroy(conn.identity.gameObject);

             NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
         }
     }

     base.ServerChangeScene(newSceneName);
 }

 public override void OnServerSceneChanged(string sceneName)
 {
     if (sceneName.StartsWith("Scene_Map"))
     {
         GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
         NetworkServer.Spawn(playerSpawnSystemInstance);

         GameObject roundSystemInstance = Instantiate(roundSystem);
         NetworkServer.Spawn(roundSystemInstance);
     }
 }


}*/


