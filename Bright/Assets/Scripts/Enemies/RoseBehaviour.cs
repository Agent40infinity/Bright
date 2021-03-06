using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBehaviour : MonoBehaviour
{
    public float movementSpeed;
    //public float minMovementRange;
    public float movementRange;
    public GameObject projectile;
    public int damageToPlayer;
    private Vector3[] path;
    private int targetWaypoint;
    const float minUpdatePath = 4f;
    private Transform targetPlayer;
    private Transform currentTargetLocation;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        Vector2 moveLocation = Random.insideUnitCircle * movementRange;
        PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
        while (true)
        {
            Instantiate(projectile, targetPlayer.position, Quaternion.identity);
            yield return new WaitForSeconds(minUpdatePath); 
            moveLocation = Random.insideUnitCircle * movementRange;
            PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
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

            transform.position = Vector3.MoveTowards(transform.position, curWaypoint, movementSpeed * Time.deltaTime);
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