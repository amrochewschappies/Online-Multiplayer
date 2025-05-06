using UnityEngine;
using Mirror;
using TMPro;

public class PlayerCount : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCountChanged))]
    public int playerCount = 0;

    public TMP_Text PlayerText;
    
    void OnCountChanged(int oldCount, int newCount)
    {
        UpdatePlayerCountUI();
    }

    void UpdatePlayerCountUI()
    {
        if (PlayerText != null)
        {
            PlayerText.text = $"Players: {playerCount}";
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdatePlayerCountUI();
    }
}