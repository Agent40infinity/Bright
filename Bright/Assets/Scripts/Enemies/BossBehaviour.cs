using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("Attributes")]
    public float speed;
    public float movementRange;
    public BossPhase phase;

    [Header("References")]
    public Animator anim;
    public EnemyHealth bossHealth;
    public LineRenderer laser;

    [Header("Prefabs")]
    public GameObject enemyPrefab;


    private Transform targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        bossHealth = gameObject.GetComponent<EnemyHealth>();
        phase = BossPhase.Idle;
        laser.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        PhaseCheck();

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, targetPlayer.position);
    }

    public void PhaseCheck()
    {
        switch (phase)
        {
            case BossPhase.Idle:
                Debug.Log("Boss is currently idle");
                if (bossHealth.curHealth < bossHealth.maxHealth)
                {
                    phase = BossPhase.First;
                }
                break;
            case BossPhase.First:
                Debug.Log("Boss is now on the first phase");
                if (bossHealth.curHealth <= bossHealth.maxHealth * 0.5)
                {
                    phase = BossPhase.Shield;
                }
                break;
            case BossPhase.Shield:
                Debug.Log("Boss is now on the shield phase and does not take damage from the player");
                bossHealth.TriggerImmunity(); 
                // add method here for spawning enemies
                break;
            case BossPhase.Second:
                Debug.Log("Boss is now on the final phase");
                break;
        }
    }

    public void ShieldPhase()
    {

    }
}

public enum BossPhase
{
    Idle, 
    First,
    Shield,
    Second,
}
