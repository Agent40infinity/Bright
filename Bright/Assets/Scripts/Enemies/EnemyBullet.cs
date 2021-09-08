using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float movementSpeed;

    private Transform target;
    private Vector2 targetLoc;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetLoc = new Vector2(target.position.x, target.position.y);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetLoc, movementSpeed * Time.deltaTime);

        if(transform.position.x == targetLoc.x && transform.position.y == targetLoc.y)
        {
            Explode();
        }
    }

    void Explode()
    {
        // implement particle pew pew
    }
}
