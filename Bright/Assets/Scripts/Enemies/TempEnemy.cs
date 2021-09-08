using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public int health;

    public float movementSpeed;
    public float retreatSpeed;
    public float targetDistance;
    public float tooClose;

    private float shotTimer;
    public float timeBetweenShots;

    public GameObject bullet;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        shotTimer = timeBetweenShots;
    }

    void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        if (shotTimer >= 0)
        {
            shotTimer -= Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, target.position) > targetDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        }
        else if((Vector2.Distance(transform.position, target.position) < targetDistance) && (Vector2.Distance(transform.position, target.position) > tooClose) && shotTimer <= 0)
        {
            Shoot();
        }
        else if(Vector2.Distance(transform.position, target.position) < tooClose)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -retreatSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
         Instantiate(bullet, transform.position, Quaternion.identity);
         shotTimer = timeBetweenShots;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
