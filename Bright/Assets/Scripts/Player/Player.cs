using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WispMode wispMode = WispMode.Idle;
    public WispMode previousMode;

    public PlayerPhysics physics;

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

    public void Blade()
    {
        physics.PlayerRotation(WispMode.Blade);

        if (Input.GetKeyDown(GameManager.keybind["Melee"]))
        {

        }
    }

    public void Projectile()
    {
        physics.PlayerRotation(WispMode.Projectile);
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