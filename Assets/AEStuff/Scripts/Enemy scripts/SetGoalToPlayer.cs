using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetGoalToPlayer : MonoBehaviour {
    private NavMeshAgent agent;
    private float AgroRange;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        EnemyStats stats = GetComponent<EnemyStats>();
        if (stats == null)
        {
            Debug.Log("No stats component attached to enemy ");
        }
        AgroRange = stats.AgroRange;
    }
	
	// Update is called once per frame
	void Update () {
        // Find the closest player and set him as goal
        float closestDistance = AgroRange;
        int closestPlayerIndex = -1;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.GetLength(0); i++)
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position);
            if (distance<closestDistance)
            {
                closestDistance = distance;
                closestPlayerIndex = i;
            }
        }
        if (closestPlayerIndex != -1)
        {
            agent.destination = players[closestPlayerIndex].transform.position;
        }
	}
}
