using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyType;
    // In seconds
    public float enemiesPerSecond;
    public int amountToSpawn;
    public float spawnRange;

    private float spawnTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        if (amountToSpawn > 0 && spawnTimer > 1/enemiesPerSecond)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.GetLength(0); i++)
            {
                float distance = Vector3.Distance(players[i].transform.position, transform.position);
                if (distance < spawnRange)
                {
                    amountToSpawn--;
                    spawnTimer = 0;
                    Instantiate(enemyType, transform.position, transform.rotation);
                    // disable script if we dont have anything left to spawn
                    if (amountToSpawn == 0)
                    {
                        GetComponent<EnemySpawner>().enabled = false;
                    }
                }
            }
        }
    }
}
