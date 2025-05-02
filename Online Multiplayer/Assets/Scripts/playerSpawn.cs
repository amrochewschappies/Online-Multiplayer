using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class playerSpawn : MonoBehaviour
{
/*
    public List<Transform> spawnPoints = new List<Transform>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetAvailableSpawnPoint(); // Your custom logic
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    private Transform GetAvailableSpawnPoint()
    {
        // Example: random spawn point
        if (spawnPoints.Count == 0) return null;
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }*/
}
