using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>(true); 

        if (!isLocalPlayer && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
        }
        else if (isLocalPlayer && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
}