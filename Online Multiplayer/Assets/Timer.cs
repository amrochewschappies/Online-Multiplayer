using UnityEngine;
using UnityEngine;
using Mirror;
using TMPro;

public class Timer : NetworkBehaviour
{
    //Timer script take from Andy
    [SyncVar(hook = nameof(OnTimeChanged))]
    public float currentTime = 60f;

    public TMP_Text timerText;
    public bool isCountingDown = true;

    void Update()
    {
        if (!isServer) return;

        int playerCount = CustomNetworkManager.instance != null ? CustomNetworkManager.instance.Players.Count : 0;
        
        if (playerCount != 2)
        {
            if (currentTime != 60f) 
            {
                currentTime = 60f;
                isCountingDown = false;
            }
            return;
        }
        
        if (!isCountingDown)
            isCountingDown = true;

        if (isCountingDown && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f && playerCount == 2)
            {
                currentTime = 0f;
                isCountingDown = false;

                NetworkManager.singleton.ServerChangeScene("GameScene");
            }
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
