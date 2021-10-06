﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMushroom : MonoBehaviour
{
    public float movementSpeed;

    public int damageToPlayer;

    const float minUpdatePath = 0.1f;
    //public float updatePath;
    private Vector3[] path;
    private int targetWaypoint;

    private Rigidbody2D rb;
    private Transform targetPlayer;
    private Transform currentTargetLocation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
       // PathfindingManager.PathRequest(transform.position, targetPlayer.position, OnPathFound);
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
