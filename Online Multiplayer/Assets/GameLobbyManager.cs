// GameLobbyManager.cs
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLobbyManager : NetworkRoomManager
{
    private LobbyState lobbyState;
    public static GameLobbyManager Instance;

    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (NetworkServer.active && SceneManager.GetActiveScene().name == RoomScene)
            SpawnLobbyState();
        
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        if (sceneName == RoomScene)
            SpawnLobbyState();
    }

    private void SpawnLobbyState()
    {
        // find any existing one…
        lobbyState = FindObjectOfType<LobbyState>();
        if (lobbyState == null)
        {
            // …or create it only if it doesn’t exist
            var go = new GameObject("LobbyState");
            go.AddComponent<NetworkIdentity>();
            lobbyState = go.AddComponent<LobbyState>();

            // initialize *once* here, before spawning
            lobbyState.InitializeState(numPlayers: 0);

            NetworkServer.Spawn(go);
        }
    }


    // === THIS FIRES WHEN Mirror HAS INCREMENTED numPlayers ===
    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);
        Debug.Log($"[Server] OnRoomServerAddPlayer → numPlayers = {numPlayers}");
        lobbyState.UpdatePlayerCount(numPlayers);
    }

    // === THIS FIRES WHEN Mirror HAS DECREMENTED numPlayers ===
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);
        lobbyState.UpdatePlayerCount(numPlayers);
    }

    // No need to override OnRoomServerConnect for player‐count updates.
}