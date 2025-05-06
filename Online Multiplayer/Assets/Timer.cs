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
        if (!isServer) return;
        if (isCountingDown && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isCountingDown = false;

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
            timerText.text = timeToShow.ToString("Game Starts in:"+"f0");
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateTimerUI(currentTime);
    }
}
