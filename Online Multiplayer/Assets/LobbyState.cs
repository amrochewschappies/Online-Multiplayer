// LobbyState.cs

using Mirror;
using UnityEngine;
using TMPro;

// This component handles all synchronized lobby state and UI updates
[RequireComponent(typeof(NetworkIdentity))]
public class LobbyState : NetworkBehaviour
{
    [Header("UI References (Client Only)")] [SerializeField]
    private TMP_Text playerCountText;

    [SerializeField] private TMP_Text timerText;

    [Header("Settings")] [SerializeField] private float lobbyCountdown = 10f;

    // SyncVars moved here
    [SyncVar(hook = nameof(OnPlayerCountChanged))]
    private int currentPlayerCount = 0;

    [SyncVar(hook = nameof(OnTimerValueChanged))]
    private float timer;

    [SyncVar(hook = nameof(OnTimerRunningChanged))]
    private bool timerRunning = false;

    #region Server Logic

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeState(numPlayers: 0);
    }

    [Server]
    public void InitializeState(int numPlayers)
    {
        currentPlayerCount = numPlayers;
        timer = lobbyCountdown;
        timerRunning = false;
    }

    [Server]
    public void UpdatePlayerCount(int newCount)
    {
        currentPlayerCount = newCount;
        CheckStartConditions();
    }

    [ServerCallback]
    private void Update()
    {
        if (!timerRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (currentPlayerCount >= 2)
            {
                timerRunning = false;
                RpcStartGame();
            }
            else
            {
                timerRunning = false;
                timer = lobbyCountdown;
            }
        }
    }

    [Server]
    private void CheckStartConditions()
    {
        if (currentPlayerCount >= 2)
        {
            timerRunning = true;
            timer = lobbyCountdown;
        }
        else
        {
            timerRunning = false;
            timer = lobbyCountdown;
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        var mgr = NetworkManager.singleton as NetworkRoomManager;
        if (mgr != null)
            mgr.ServerChangeScene(mgr.GameplayScene);
    }

    #endregion

    #region Client Hooks & UI

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnPlayerCountChanged(0, currentPlayerCount);
        OnTimerValueChanged(0, timer);
        OnTimerRunningChanged(false, timerRunning);
    }

    private void OnPlayerCountChanged(int oldCount, int newCount)
    {
        if (playerCountText != null)
            playerCountText.text = $"Players in Lobby: {newCount}";
    }

    private void OnTimerValueChanged(float oldTime, float newTime)
    {
        if (timerText == null) return;
        timerText.text = timerRunning ? $"Starting in: {Mathf.CeilToInt(newTime)}s" : "Waiting for players…";
    }

    private void OnTimerRunningChanged(bool oldVal, bool newVal)
    {
        OnTimerValueChanged(timer, timer);
    }

    #endregion
}