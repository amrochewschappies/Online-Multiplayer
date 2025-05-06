using UnityEngine;
using TMPro;
using Mirror;
using System.Net;
using System.Net.Sockets;

public class HostIPDisplay : MonoBehaviour
{
    void Start()
    {
        var ipText = GetComponent<TextMeshProUGUI>();

        if (NetworkServer.active)
        {
            string ip = GetLocalIPAddress();
            ipText.text = $"Host IP: {ip}";
            ipText.gameObject.SetActive(true);
        }
        else
        {
            ipText.gameObject.SetActive(false);
        }
    }

    private string GetLocalIPAddress()
    {
        foreach (var ip in System.Net.Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }

        return "Unavailable";
    }
}