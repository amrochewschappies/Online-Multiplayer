using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public List<Transform> spawnPoints = new List<Transform>();  // List of spawn points
    public List<GameObject> Players = new List<GameObject>(); // List of players
    private bool gameStarted = false; 
    private int playerAmount = 0; 

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Make sure we have valid spawn points
        if (spawnPoints.Count == 0)
        {
            FindSpawnPoints();  // Dynamically find them
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available! Cannot spawn player.");
            return;
        }

        // Get a spawn point based on connection count or player count
        int spawnIndex = NetworkServer.connections.Count - 1;
        Transform spawnPoint = spawnPoints[spawnIndex % spawnPoints.Count];

        // Instantiate and add player
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // Track the player
        Players.Add(player);

        // Optional: assign teams
        SortPlayers();
    }

    private void FindSpawnPoints()
    {
        // Find all objects with the tag "SpawnPoint" in the current scene
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        // Clear the list first
        spawnPoints.Clear();

        foreach (GameObject spawnObject in spawnObjects)
        {
            spawnPoints.Add(spawnObject.transform);
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points found in the scene!");
        }
    }

    private void SortPlayers()
    {
        if (Players.Count == 0) return;

        int half = Players.Count / 2;

        for (int i = 0; i < Players.Count; i++)
        {
            if (i < half)
            {
                // Assign to team 1
                Players[i].tag = "Team1";
            }
            else
            {
                // Assign to team 2
                Players[i].tag = "Team2";
            }

            playerAmount += 1;
        }
    }
    // You can optionally add a method to handle scene changes and find spawn points in the new scene
    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);

        // Find spawn points after changing to the new scene
        if (newSceneName == "GameScene")
        {
            FindSpawnPoints();
        }
    }
}
