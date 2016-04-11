using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public int maxHealth = 100;
    public Vector3 spawnPosition = Vector3.zero;
    // Will cause the server to update clients when changed
    [SyncVar (hook = "OnChangeHealth")] public int currentHealth = 100;

    public bool destroyOnDeath = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Testing to take damage
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TakeDamage(50);
        }
	}

    public void TakeDamage(int amount)
    {
        // Only server will manage damage
        if(!isServer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
    }

    void OnChangeHealth(int health)
    {
        // If we want to update healthbar here
        currentHealth = health;
    }

    // Only client want to reset position, since he got authority of the player itself
    [ClientRpc]
    void RpcRespawn()
    {
        GameObject spawnLocation = GameObject.FindGameObjectWithTag("SpawnLocation");
        if (spawnLocation != null)
        {
            transform.position = spawnLocation.transform.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }
}
