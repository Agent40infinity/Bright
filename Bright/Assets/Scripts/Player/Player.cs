using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 input;
    public float speed;
    public Rigidbody2D rigid;

    public InteractState interactState = InteractState.Idle;

    public WispMode wispMode = WispMode.Idle;
    public WispMode previousMode;

    public void Update()
    {
        Keypress();
        Interaction();

        switch (interactState)
        {
            case InteractState.Idle:
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                break;
            case InteractState.Occupied:
                input = Vector2.zero;
                break;
        }

        switch (wispMode)
        {
            case WispMode.Blade:
                Blade();
                break;
            case WispMode.Projectile:
                Projectile();
                break;
        }
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
            interactState = InteractState.Occupied;

            Healing();
        }
        else if (Input.GetKeyUp(GameManager.keybind["Heal"]))
        {
            wispMode = previousMode;
            interactState = InteractState.Idle;

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

    public void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        rigid.velocity = input * speed;
    }



    public void Blade()
    {
        if (Input.GetKeyDown(GameManager.keybind["Melee"]))
        {

        }
    }

    public void Projectile()
    {
        if (Input.GetKey(GameManager.keybind["ShootUp"]))
        {

        }
        else if (Input.GetKey(GameManager.keybind["ShootDown"]))
        { 
        
        }
        else if (Input.GetKey(GameManager.keybind["ShootLeft"]))
        {

        }
        else if (Input.GetKey(GameManager.keybind["ShootRight"]))
        {

        }
    }

    public void Healing()
    {

    }

    public void ResetHealing()
    { 
    
    }
}

public enum InteractState
{ 
    Idle,
    Occupied
}

public enum WispMode
{ 
    Blade,
    Projectile,
    Healing,
    Idle
}