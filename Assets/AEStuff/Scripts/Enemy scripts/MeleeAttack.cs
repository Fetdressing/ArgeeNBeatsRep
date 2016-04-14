using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {
    private int damage;
    private float attackSpeed;
    private float attackRange;
    private float attackTimer = 0;
	// Use this for initialization
	void Start () {
        EnemyStats stats = GetComponent<EnemyStats>();
        if (stats == null)
        {
            Debug.Log("No stats component attached to enemy ");
        }
        else
        {
            damage = stats.AttackDamage;
            attackSpeed = stats.AttackSpeed;
            attackRange = stats.AttackRange;
        }
    }
	
	// Update is called once per frame
	void Update () {
        attackTimer += Time.deltaTime;
        if (attackTimer > 1 / attackSpeed)
        {
            // Perform attack if any player is near by
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.GetLength(0); i++)
            {
                float distance = Vector3.Distance(players[i].transform.position, transform.position);
                if (distance < attackRange)
                {
                    attackTimer = 0;
                    // Perform attack here! Only attack the first player we find in range, can be changed to attack the one with low healt
                    players[i].GetComponent<Health>().TakeDamage(damage);
                    break;
                }
            }
        }
	}
}
