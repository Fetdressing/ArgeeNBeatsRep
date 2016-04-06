using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public int maxHealth = 100;

    // Will cause the server to update clients when changed
    [SyncVar (hook = "OnChangeHealth")] public int currentHealth = 100;

    public bool destroyOnDeath = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
        transform.position = Vector3.zero;
    }
}
