using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspBehaviour : MonoBehaviour
{
    public float movementSpeed;
    public float targetDistance;
    //public float minimumDistance;
    public float shotDelay;
    public GameObject projectile;
    public int damageToPlayer;
    private float shotCooldown;
    private Vector3[] path;
    private int targetWaypoint;
    const float minUpdatePath = 0.1f;
    private Rigidbody2D rb;
    private Transform targetPlayer;
    private Transform currentTargetLocation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        PathfindingManager.PathRequest(transform.position, targetPlayer.position, OnPathFound);

        while (true)
        {
            yield return new WaitForSeconds(minUpdatePath);
            PathfindingManager.PathRequest(transform.position, targetPlayer.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathActive)
    {
        if (pathActive)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        //float distanceFromPlayer = Vector2.Distance(targetPlayer.position, transform.position);
        Vector3 curWaypoint = path[0];
        while (true)
        {
            if (transform.position == curWaypoint)
            {
                targetWaypoint++;
                if (targetWaypoint >= path.Length)
                {
                    yield break;
                }
                curWaypoint = path[targetWaypoint];
            }

            if (Vector2.Distance(targetPlayer.position, transform.position) > targetDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, curWaypoint, movementSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(targetPlayer.position, transform.position) < 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, curWaypoint, -movementSpeed * Time.deltaTime);
            }
            else
            {
                if (shotCooldown <= 0)
                {
                    shotCooldown = shotDelay;

                    EnemyBullet bullet = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<EnemyBullet>();
                    bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, GameObject.FindWithTag("Player").transform.position - transform.position);
                    bullet.shotDirection = Vector2.up;
                }
                else
                {
                    shotCooldown -= Time.deltaTime;
                }
            }
            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(damageToPlayer);
        }
    }
}
