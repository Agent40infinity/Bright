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

    [Header("Files")]
    public string animPath = "Animations/Enemy/";

    [Header("References")]
    public Animator anim;
    public EnemyHealth health;

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
                break;
            case EnemyType.Flower:
                weighting = 2;
                maxHealth = 4;
                speed = 4;
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
    }

    public void Update()
    {
        Pathfinding();
    }

    public void Pathfinding()
    { 
        switch (enemyType)
        { 
            case EnemyType.Mushroom: case EnemyType.Wasp: case EnemyType.Flower: case EnemyType.SeedBomb:
                
                break;  
        }
    }

    public IEnumerator FollowPath()
    {
        switch (enemyType)
        {
            case EnemyType.Mushroom:

                break;
            case EnemyType.Wasp:

                break;
            case EnemyType.Flower:

                break;
            case EnemyType.SeedBomb:

                break;
        }

        yield return null;
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