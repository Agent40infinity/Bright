using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMushroom : MonoBehaviour
{
    public float delayBetweenJumps;
    public float jumpSpeed;
    public float jumpDistance;
    public float targetDistance;
    public float delayBetweenAttacks;

    private float jumpTimer;
    private float attackTimer;

    public GameObject bullet;

    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        jumpTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        rb.velocity = rb.velocity * 0.99f;

        if (Vector2.Distance(transform.position, target.position) > targetDistance && jumpTimer <= 0)
        {
            jumpTimer = delayBetweenJumps;
            Jump();
        }
        else if (Vector2.Distance(transform.position, target.position) < targetDistance && attackTimer <=0)
        {
            attackTimer = delayBetweenAttacks;
            Attack();
        }
    }

    void Jump()
    {
        var direction = Vector3.zero;
        direction = target.position - transform.position;
        rb.AddRelativeForce(direction.normalized * jumpSpeed * jumpDistance, ForceMode2D.Force);
    }

    void Attack()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        // add pew pew sound effect later xd 
    }

    // damage the player if they run into the mushroom
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(1);
        }
    }
}
