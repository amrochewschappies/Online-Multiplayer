using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (ManageLobby.Instance != null)
        {
            ManageLobby.Instance.AddPlayer(netIdentity, playerName);
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        if (ManageLobby.Instance != null)
        {
            ManageLobby.Instance.RemovePlayer(netIdentity);
        }
    }

    public override void OnStartLocalPlayer()
    {
        CmdSetName(PlayerNameHolder.playerName);
    }

    [Command]
    void CmdSetName(string name)
    {
        playerName = name;
    }
}