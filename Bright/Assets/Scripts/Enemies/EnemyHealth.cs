using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 6;
    public int curHealth; 

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
        if (curHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        curHealth--;
    }
}
