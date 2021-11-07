using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Attributes")]
    public float maxHealth;
    public float speed;
    public float movementRange; //applies only to specific enem

    [Header("Files")]
    public string animPath = "Animations/Enemy/";

    [Header("References")]
    public Animator anim;
    public EnemyHealth health;

    [Header("Attacking Details")]
    public float maxShotDelay;
    public float minShotDelay;
    private float shotCooldown;
    public GameObject projectilePrefab;
    public GameObject thornPrefab;
    public GameObject seedlingPrefab;
    public float targetDistance;

    [Header("Pathfinding")]
    private Transform targetPlayer;
    private Transform currentTargetLocation;    
    private Vector3[] path;
    private int targetWaypoint;
    private float pathRefreshRate = 0.1f;

    public void EnemySetup(EnemyType enemyType)
    {
        this.enemyType = enemyType;
        animPath += enemyType.ToString();
        anim.runtimeAnimatorController = Resources.Load(animPath) as RuntimeAnimatorController;

        switch (enemyType)
        {
            case EnemyType.Mushroom:
                maxHealth = 6;
                speed = 2;
                break;
            case EnemyType.Wasp:
                maxHealth = 6;
                speed = 3;
                maxShotDelay = .8f;
                targetDistance = 2;
                break;
            case EnemyType.Flower:
                maxHealth = 4;
                speed = 3;
                pathRefreshRate = 5;
                break;
            case EnemyType.SeedBomb:
                maxHealth = 2;
                speed = 4;
                minShotDelay = 1.5f;
                maxShotDelay = 4f;
                pathRefreshRate = 2.5f;
                break;
            case EnemyType.Sunflower:
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
        StartCoroutine("Awaken");
    }

    public IEnumerator Awaken()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        switch (enemyType)
        {
            case EnemyType.Mushroom: case EnemyType.Wasp: 

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
                    Instantiate(thornPrefab, targetPlayer.position, Quaternion.identity);
                    yield return new WaitForSeconds(pathRefreshRate);
                    moveLocation = Random.insideUnitCircle * movementRange;
                    PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
                }

            case EnemyType.SeedBomb:
                Vector2 randLocation = Random.insideUnitCircle * movementRange;
                PathfindingManager.PathRequest(transform.position, randLocation, OnPathFound);
                while (true)
                {
                    yield return new WaitForSeconds(pathRefreshRate);
                    moveLocation = Random.insideUnitCircle * movementRange;
                    PathfindingManager.PathRequest(transform.position, moveLocation, OnPathFound);
                }

            case EnemyType.Sunflower:
                while (true)
                {
                    // no move
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
                    if (Vector2.Distance(targetPlayer.position, transform.position) > targetDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                    }
                    else if (Vector2.Distance(targetPlayer.position, transform.position) < targetDistance * 0.75f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, curWaypoint, -speed * Time.deltaTime);
                    }
                    else
                    {
                        if (shotCooldown <= 0)
                        {
                            shotCooldown = maxShotDelay;

                            EnemyBullet bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<EnemyBullet>(); 
                            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, GameObject.FindWithTag("Player").transform.position - transform.position); 
                            bullet.shotDirection = Vector2.up; 
                        }
                        else
                        {
                            shotCooldown -= Time.deltaTime;
                        }
                    }
                    break;
                case EnemyType.Flower:
                    transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                    break;
                case EnemyType.SeedBomb:
                    transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
                    if (shotCooldown <= 0)
                    {
                        shotCooldown = Random.Range(minShotDelay, maxShotDelay);

                        Instantiate(seedlingPrefab, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        shotCooldown -= Time.deltaTime;
                    }
                    break;
                case EnemyType.Sunflower:
                    // lololol 
                    break;
            }
            yield return null;
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