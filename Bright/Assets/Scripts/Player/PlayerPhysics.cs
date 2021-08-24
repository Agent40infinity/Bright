using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 input;
    public float speed;
    public float slowMultiplier;
    public float slowApplied;

    [Header("Reference")]
    public Rigidbody2D rigid;
    public Transform head;
    public Animator anim;
    
    [Header("Interaction")]
    public InteractState interactState = InteractState.Idle;

    public void Update()
    {
        Keypress();

        switch (interactState)
        {
            case InteractState.Idle:
                slowApplied = 1;
                break;
            case InteractState.Slowed:
                slowApplied = slowMultiplier;
                break;
            case InteractState.Occupied:
                input = Vector2.zero;
                break;
        }
    }

    public void PlayerRotation(WispMode mode)
    {
        switch (mode)
        {
            case WispMode.Blade:
                if (Input.GetKey(GameManager.keybind["MoveUp"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if (Input.GetKey(GameManager.keybind["MoveDown"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (Input.GetKey(GameManager.keybind["MoveLeft"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 270);
                }
                else if (Input.GetKey(GameManager.keybind["MoveRight"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
            case WispMode.Projectile:
                if (Input.GetKey(GameManager.keybind["ShootUp"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 180);
                }
                else if (Input.GetKey(GameManager.keybind["ShootDown"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (Input.GetKey(GameManager.keybind["ShootLeft"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 270);
                }
                else if (Input.GetKey(GameManager.keybind["ShootRight"]))
                {
                    head.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;
        }
    }

    public void Keypress()
    {
        if (Input.GetKey(GameManager.keybind["MoveUp"]))
        {
            anim.SetBool("Facing", true);
            input.y = 1;
        }
        else if (Input.GetKey(GameManager.keybind["MoveDown"]))
        {
            anim.SetBool("Facing", false);
            input.y = -1;
        }
        else
        {
            input.y = 0;
        }

        if (Input.GetKey(GameManager.keybind["MoveLeft"]))
        {
            input.x = -1;
        }
        else if (Input.GetKey(GameManager.keybind["MoveRight"]))
        {
            input.x = 1;
        }
        else
        {
            input.x = 0;
        }
    }

    public void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        rigid.velocity = (input * slowApplied) * speed;
    }
}

public enum InteractState
{
    Idle,
    Slowed,
    Occupied
}