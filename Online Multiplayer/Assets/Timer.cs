using UnityEngine;
using UnityEngine;
using Mirror;
using TMPro;

public class Timer : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnTimeChanged))]
    public float currentTime = 60f;

    public TMP_Text timerText;
    public bool isCountingDown = true;

    void Update()
    {
        if (!isServer || CustomNetworkManager.instance == null) return;

        int count = CustomNetworkManager.instance.roomSlots.Count;

        if (count < 2 || count == 3)
        {
            ResetTimer();
            return;
        }
        
        if (isCountingDown && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isCountingDown = false;
                StartGameIfValid(count);
            }
        }
        else if (!isCountingDown)
        {
            isCountingDown = true; 
        }
    }

    void ResetTimer()
    {
        currentTime = 60f;
        isCountingDown = false;
    }

    void StartGameIfValid(int playerCount)
    {
        if (playerCount >= 2 && playerCount != 3)
        {
            NetworkManager.singleton.ServerChangeScene("GameScene");
        }
        else
        {
            ResetTimer();
        }
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        CustomNetworkManager.OnPlayerReady += CheckStartConditions;
    }

    [ServerCallback]
    void CheckStartConditions()
    {
        if (CustomNetworkManager.instance.roomSlots.Count >= 2)
        {
            StartCountdown();
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        CustomNetworkManager.OnPlayerReady -= CheckStartConditions;
    }
    [Server]
    public void StartCountdown()
    {
        if (!isCountingDown && CustomNetworkManager.instance.roomSlots.Count >= 2)
        {
            isCountingDown = true;
            Debug.Log("Countdown started.");
        }
    }

    void OnTimeChanged(float oldTime, float newTime)
    {
        UpdateTimerUI(newTime);
    }

    void UpdateTimerUI(float timeToShow)
    {
        if (timerText != null)
        {
            timerText.text = "Game starts in: " + timeToShow.ToString("F0");
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateTimerUI(currentTime);
    }
}