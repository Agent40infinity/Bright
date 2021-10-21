using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 6;
    public int curHealth;

    private bool immunity;

    public void Start()
    {
        curHealth = maxHealth;
    }

    public void Update()
    {
        HealthCheck();
    }

    public void HealthCheck()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Death();
        }

        if (curHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        EnemyManager manager = GetComponentInParent<EnemyManager>();
        manager.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        curHealth--;
    }

    public void TriggerImmunity()
    {
        immunity = true;
    }

    public void RemoveImmunity()
    {
        immunity = false;
    }

    // Temporary damage takes damage each time hit by player
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wisp" && immunity != true)
        {
            TakeDamage(1);
        }
        else {
            // take no damage from running into other shit 
        }
    }
}
