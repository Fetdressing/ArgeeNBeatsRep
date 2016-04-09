using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

    public static List<GameObject> playerList = new List<GameObject>();
    public int numPlayers = 0;

    void Start()
    {
        playerList.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }
    }

    void OnPlayerConnected()
    {
        playerList.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        playerList.Clear();
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }
    }

    // Update is called once per frame
    void Update () {

	}
}
