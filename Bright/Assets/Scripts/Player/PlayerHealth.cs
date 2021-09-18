using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int curHealth; //Value for player's current health.
    public int maxHealth = 3; //default value for player's max health.

    [Header("iFrame")]
    public float iFrameTimerReset;
    private float iFrameTimer; //Counter for iFrame activation.
    public FrameState frameState;

    [Header("Reference")]
    public PlayerPhysics physics;

    public void Awake()
    {
        curHealth = maxHealth;
    }

    public void Update()
    {
        HealthManagement();
    }

    public void HealthManagement() //deals with health deduction and external UI changes.
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DamagePlayer(1);
        }

        IFrame();

        if (curHealth <= 0) //Checks whether or not the curret health is less than or equal to 0 and that fadeIntoDeath has not already been activated.
        {
            physics.interactState = InteractState.Occupied;
            //StartCoroutine("Death");
        }
    }

    public void IFrame() //deals with the iFrame controller after the activation of certain abilities and actions.
    {
        switch (frameState)
        {
            case FrameState.Active:
                iFrameTimer -= Time.deltaTime;
                if (iFrameTimer < 0) //Checks that enough time has passed so the iFrame can end and allow the player to take damage again.
                {
                    iFrameTimer = iFrameTimerReset;
                    frameState = FrameState.Idle;
                }
                break;
        }
    }

    public void DamagePlayer(int damage)
    {
        switch (frameState)
        {
            case FrameState.Idle:
                if (curHealth >= 1)
                {
                    curHealth--;
                    if (curHealth != 0) //Applies knockback to the player only while their health isn't 0.
                    {
                        frameState = FrameState.Active;
                    }
                }
                break;
        }
    }
}

public enum FrameState
{
    Active,
    Idle
}
