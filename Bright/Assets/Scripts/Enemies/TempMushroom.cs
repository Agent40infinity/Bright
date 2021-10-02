using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMushroom : MonoBehaviour
{
    public float delayBetweenJumps;
    public float jumpDistance;
    public float targetDistance;
    public float delayBetweenAttacks;

    private float jumpTimer;
    private float attackTimer;

    public GameObject bullet;

    private Rigidbody2D rb;
    private Transform targetPlayer;

    [Header("Avoidance")]
    public float raycastDistance;
    public LayerMask walls;
    private bool wallCheck;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        jumpTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        rb.velocity = rb.velocity * 0.99f;

        if (Vector2.Distance(transform.position, targetPlayer.position) > targetDistance && jumpTimer <= 0)
        {
            jumpTimer = delayBetweenJumps;
            MoveCheck();
        }
        else if (Vector2.Distance(transform.position, targetPlayer.position) < targetDistance && attackTimer <=0)
        {
            attackTimer = delayBetweenAttacks;
            Attack();
        }
    }

    void MoveCheck()
    {
        var direction = Vector3.zero;
        direction = targetPlayer.position - transform.position;
        wallCheck = Physics2D.Raycast(transform.position, direction, raycastDistance, walls);


        if(wallCheck == true)
        {
            Debug.Log("i hit a wall lol");
            
        }
        else
        {
            rb.AddRelativeForce(direction.normalized * jumpDistance, ForceMode2D.Force);
        }
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

    public enum BehaviourState
    {
        Aggressive,
        Retreat,
        Idle
    }
}
