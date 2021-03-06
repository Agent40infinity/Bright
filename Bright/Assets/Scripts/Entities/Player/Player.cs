using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Wisp/Weapon Mode")]
    public WispMode wispMode = WispMode.Idle;
    public ReturnState returnState = ReturnState.Idle;
    private WispMode previousMode;

    public Vector2 direction;

    [Header("Melee")]
    public float attackInterval;
    public float attackRange;
    public int damage;
    private MeleeMode meleeMode;

    [Header("Perks")]
    public List<Perk> perkList = new List<Perk>();

    [Header("Reference")]
    public PlayerPhysics physics;
    public WispPhysics wispPhysics;

    public void Update()
    {
        Keypress();
        Interaction();

        switch (wispMode)
        {
            case WispMode.Blade:
                Blade();
                break;
            case WispMode.Projectile:
                Projectile();
                break;
        }
        TrueSight();
        /*switch (GameManager.enemyManager.spawnerState)
        {
            case SpawnerState.Cleared:
                TrueSight();
                break;
        }*/
    }

    public void Keypress()
    {
        if (Input.GetKey(GameManager.keybind["Heal"]))
        {
            if (wispMode != WispMode.Healing)
            {
                previousMode = wispMode;
            }
            wispMode = WispMode.Healing;
            physics.interactState = InteractState.Slowed;

            Healing();
        }
        else if (Input.GetKeyUp(GameManager.keybind["Heal"]))
        {
            wispMode = previousMode;
            physics.interactState = InteractState.Idle;

            ResetHealing();
        }

        if (Input.GetKeyDown(GameManager.keybind["SwitchWeapon"]))
        {
            if (wispMode == WispMode.Blade)
            {
                wispMode = WispMode.Projectile;
            }
            else
            {
                wispMode = WispMode.Blade;
            }
        }
    }

    public void Interaction()
    {
        if (Input.GetKeyDown(GameManager.keybind["Interact"]))
        {

        }
    }

    public void TrueSight()
    {
        if (Input.GetKeyDown(GameManager.keybind["TrueSight"]))
        {
            switch (GameManager.worldState)
            {
                case WorldState.Normal:
                    GameManager.worldState = WorldState.Other;
                    break;
                case WorldState.Other:
                    GameManager.worldState = WorldState.Normal;
                    break;
            }

            GameManager.startTransition = true;
        }
    }

    public void Blade()
    {
        physics.PlayerRotation(WispMode.Blade);

        switch (meleeMode)
        {
            case MeleeMode.Idle:
                if (Input.GetKeyDown(GameManager.keybind["Melee"]))
                {
                    StartCoroutine("Melee");
                }
                break;
        }
    }

    public IEnumerator Melee()
    {
        physics.anim.SetBool("Melee", true);
        meleeMode = MeleeMode.Attack;

        Vector2 attackPos = physics.head.GetChild(0).position;
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPos, attackRange);

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            switch (enemiesInRange[i].tag)
            {
                case "Enemy":
                    enemiesInRange[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                    break;
            }
        }
        yield return new WaitForSeconds(attackInterval);
        physics.anim.SetBool("Melee", false);
        meleeMode = MeleeMode.Idle;
    }

    public void Projectile()
    {
        physics.PlayerRotation(WispMode.Projectile);

        switch (returnState)
        {
            case ReturnState.Idle:
                wispPhysics.Follow();

                if (Input.GetKey(GameManager.keybind["ShootLeft"]))
                {
                    direction = Vector2.left;
                    returnState = ReturnState.Out;
                }
                else if (Input.GetKey(GameManager.keybind["ShootRight"]))
                {
                    direction = Vector2.right;
                    returnState = ReturnState.Out;
                }
                else if (Input.GetKey(GameManager.keybind["ShootUp"]))
                {
                    direction = Vector2.up;
                    returnState = ReturnState.Out;
                }
                else if (Input.GetKey(GameManager.keybind["ShootDown"]))
                {
                    direction = Vector2.down;
                    returnState = ReturnState.Out;
                }
                
                break;
            case ReturnState.Out:
                wispPhysics.ShootOut(direction);
                break;
        }
    }

    public void Healing()
    {

    }

    public void ResetHealing()
    { 
    
    }
}

public enum WispMode
{ 
    Blade,
    Projectile,
    Healing,
    Idle
}

public enum MeleeMode
{ 
    Idle,
    Attack
}

public enum ReturnState
{ 
    Out, 
    Idle
}