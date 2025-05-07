using Mirror;
using UnityEngine;

public class RoomLobbyPlayer : NetworkRoomPlayer
{
    // SyncVar to track readiness with a hook method
    [SyncVar(hook = nameof(ReadyStateChanged))]
    public bool readyToBegin;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Local player started in lobby.");
    }

    public override void OnStartClient()
    {
        Debug.Log("Client joined lobby.");
    }

    public override void OnStartServer()
    {
        Debug.Log("Server added player to lobby.");
    }

    private void ReadyStateChanged(bool oldValue, bool newValue)
    {
        Debug.Log($"Player {netId} ready state changed: {oldValue} → {newValue}");

        if (newValue)
        {
            CustomNetworkManager.InvokePlayerReady(); 
        }
    }

    // Called from the UI when player presses the "Ready" button
    public void SetReady()
    {
        if (isLocalPlayer)
        {
            CmdChangeReadyState(true);
        }
    }

    // Optional unready button logic
    public void SetUnready()
    {
        if (isLocalPlayer)
        {
            CmdChangeReadyState(false);
        }
    }
}