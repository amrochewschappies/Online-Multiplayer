using Mirror;
using UnityEngine;
using TMPro;  // Import TMP namespace
using UnityEngine.SceneManagement;
using System.Net;

public class GameLobbyManager : NetworkRoomManager
{
    [Header("Lobby Settings")]
    [SerializeField] private float lobbyCountdown = 10f;

    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;  // Use TMP_Text for TMP UI
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private TMP_Text hostIpText;

    private float timer;
    private bool timerRunning = false;

    public static GameLobbyManager GLMinstance;

    private void Awake()
    {
        GLMinstance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("LobbyManager server started");
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
        CheckStartConditions();
        UpdateUI();
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);
        CheckStartConditions();
        UpdateUI();
    }

    public override void OnRoomServerPlayersReady() { /* Disable Mirror ready-up */ }

    private void Update()
    {
        if (!NetworkServer.active || SceneManager.GetActiveScene().name != RoomScene) return;

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            UpdateUI();

            if (timer <= 0f)
            {
                if (numPlayers == 2 || numPlayers == 4)
                {
                    timerRunning = false;
                    ServerChangeScene(GameplayScene);
                }
                else
                {
                    Debug.Log("Invalid player count (" + numPlayers + ") — resetting timer.");
                    ResetTimer();
                    UpdateUI();
                }
            }
        }
    }

    private void CheckStartConditions()
    {
        if (numPlayers >= 2)
        {
            StartTimer();
        }
        else
        {
            ResetTimer();
        }
    }

    private void StartTimer()
    {
        if (!timerRunning)
        {
            Debug.Log("Timer started.");
            timer = lobbyCountdown;
            timerRunning = true;
        }
    }

    private void ResetTimer()
    {
        Debug.Log("Timer reset.");
        timer = lobbyCountdown;
        timerRunning = false;
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Starting in: {Mathf.CeilToInt(timer)}s";

        if (playerCountText != null)
            playerCountText.text = $"Players in Lobby: {numPlayers}";

        if (hostIpText != null)
            hostIpText.text = $"Host IP: {GetLocalIPAddress()}";
    }

    private string GetLocalIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP not found";
        }
        catch
        {
            return "IP Error";
        }
    }
}
