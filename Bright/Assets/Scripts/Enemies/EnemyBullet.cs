using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float movementSpeed;
    public int damage;

    private Transform target;
    private Vector2 targetLoc;
    private Rigidbody2D rb;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(damage);
            Explode();
        }
    }

    void Explode()
    {
        // implement particle pew pew later
        Destroy(gameObject);
    }
}
