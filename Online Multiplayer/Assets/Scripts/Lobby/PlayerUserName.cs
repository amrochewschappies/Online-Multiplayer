using Mirror;
using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerUserName : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    public TMP_Text UsernameText;

    public override void OnStartClient()
    {
        base.OnStartClient();
        UsernameText = GameObject.Find("DisplayNameText").GetComponent<TextMeshProUGUI>();

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

    void OnNameChanged(string oldName, string newName)
    {
        if (UsernameText != null)
        {
            UsernameText.text = $"Welcome {newName}! Drive Safely BOHT";
    
            
            UsernameText.gameObject.SetActive(true); 
            StartCoroutine(HideUsernameDisplayTextAfterDelay(5f)); 
        }
    }
    private IEnumerator HideUsernameDisplayTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (UsernameText != null)
        {
            Color originalColor = UsernameText.color;
            float fadeDuration = 1f; 
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                UsernameText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            UsernameText.gameObject.SetActive(false);
            UsernameText.color = originalColor; 
        }
    }
}