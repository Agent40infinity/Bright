using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Attributes")]
    public int weighting;
    public float maxHealth;
    public float speed;
    public float movementRange; //applies only to specific enem

    [Header("Files")]
    public string animPath = "Animations/Enemy/";

    [Header("References")]
    public Animator anim;
    public EnemyHealth health;

    [Header("Attacking Details")]
    public float shotDelay = 0.8f;
    private float shotCooldown;
    public GameObject projectile;
    public float targetDistance;

    [Header("Pathfinding")]
    private Transform targetPlayer;
    private Transform currentTargetLocation;
    private Vector3[] path;
    private int targetWaypoint;
    const float pathRefreshRate = 0.1f;

    public Enemy(EnemyType enemyType)
    {
        this.enemyType = enemyType;
        animPath += enemyType.ToString();
        anim.runtimeAnimatorController = Resources.Load(animPath) as RuntimeAnimatorController;

        switch (enemyType)
        {
            case EnemyType.Mushroom:
                weighting = 1;
                maxHealth = 6;
                speed = 5;
                break;
            case EnemyType.Wasp:
                weighting = 1;
                maxHealth = 6;
                speed = 5;
                projectile = Resources.Load("Enemies/Bullet") as GameObject;
                break;
            case EnemyType.Flower:
                weighting = 2;
                maxHealth = 4;
                speed = 4;
                projectile = Resources.Load("Enemies/Thorns") as GameObject;
                break;
            case EnemyType.SeedBomb:
                weighting = 2;
                maxHealth = 2;
                speed = 3;
                break;
            case EnemyType.Sunflower:
                weighting = 3;
                maxHealth = 12; 
                speed = 0;
                break;
        } 
    }

    public void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        health = gameObject.GetComponent<EnemyHealth>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    //public void Update()
    //{
    //   Pathfinding();
    //}

    //public void Pathfinding()
    //{ 
    //    switch (enemyType)
    //    { 
    //        case EnemyType.Mushroom: case EnemyType.Wasp: case EnemyType.Flower: case EnemyType.SeedBomb:
    //            
    //            break;  
    //    }
    //}

    IEnumerator UpdatePath()
    {
        switch (enemyType)
        {
            case EnemyType.Mushroom: case EnemyType.Wasp: case EnemyType.SeedBomb:

                PathfindingManager.PathRequest(transform.position, targetPlayer.position, OnPathFound);

                while (true)
                {
                    yield return new WaitForSeconds(pathRefreshRate);
                    PathfindingManager.PathRequest(transform.position, targetPlayer.position, OnPathFound);
                }

            case EnemyType.Flower:
                Vector2 moveLocation = Random.insideUnitCircle * movementRange;
                PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
                while (true)
                {
                    Instantiate(projectile, targetPlayer.position, Quaternion.identity);
                    yield return new WaitForSeconds(pathRefreshRate);
                    moveLocation = Random.insideUnitCircle * movementRange;
                    PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
                }
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

    public IEnumerator FollowPath()
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

            switch (enemyType)
            {
                case EnemyType.Mushroom:
                        transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                    break;
                case EnemyType.Wasp:
                        if (Vector2.Distance(targetPlayer.position, transform.position) < targetDistance)
                        {
                            if (shotCooldown < 0)
                            {
                                shotCooldown = shotDelay;
                                Instantiate(projectile, transform.position, Quaternion.identity);
                            }
                            else
                            {
                                shotCooldown -= Time.deltaTime;
                            }
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                        }
                    break;
                case EnemyType.Flower:
                        transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                    break;
                case EnemyType.SeedBomb:

                    break;
            }
            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(1);
        }
    }
}

public enum EnemyType
{ 
    Mushroom,
    Wasp,
    Flower,
    SeedBomb,
    Sunflower,
}