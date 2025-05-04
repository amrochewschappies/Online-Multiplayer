// GameLobbyManager.cs (updated)
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
        // Ensure a LobbyState exists in the RoomScene
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
        // Find or create the LobbyState GameObject
        lobbyState = FindObjectOfType<LobbyState>();
        if (lobbyState == null)
        {
            var go = new GameObject("LobbyState");
            var ni = go.AddComponent<NetworkIdentity>();
            lobbyState = go.AddComponent<LobbyState>();
            NetworkServer.Spawn(go);
        }
        lobbyState.InitializeState(numPlayers: numPlayers);
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
        lobbyState.UpdatePlayerCount(numPlayers);
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);
        // numPlayers has already decremented
        lobbyState.UpdatePlayerCount(numPlayers);
    }

// Other overrides (OnRoomServerPlayersReady, etc.) remain unchanged

}