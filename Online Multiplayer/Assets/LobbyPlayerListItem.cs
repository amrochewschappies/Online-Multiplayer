using UnityEngine;
using TMPro;

public class LobbyPlayerListItem : MonoBehaviour
{
    public TMP_Text playerNameText;

    public void SetName(string name)
    {
        playerNameText.text = name;
    }
}