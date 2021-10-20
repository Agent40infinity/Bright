using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 input;
    public Vector2 lastInput;
    public float speed;
    public float slowMultiplier;
    public float slowApplied;
    public float rotation;
    public bool doorActive;
    public Vector3 lerpPosition;

    [Header("Reference")]
    public Rigidbody2D rigid;
    public Transform head;
    public Animator anim;
    
    [Header("Interaction")]
    public InteractState interactState = InteractState.Idle;

    public void Update()
    {
        Keypress();

        switch (doorActive)
        {
            case true:
                LerpTransition();
                break;
        }

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
                    rotation = 180;
                }
                else if (Input.GetKey(GameManager.keybind["MoveDown"]))
                {
                    rotation = 0;
                }
                else if (Input.GetKey(GameManager.keybind["MoveLeft"]))
                {
                    rotation = 270;
                }
                else if (Input.GetKey(GameManager.keybind["MoveRight"]))
                {
                    rotation = 90;
                }
                break;
            case WispMode.Projectile:
                if (Input.GetKey(GameManager.keybind["ShootUp"]))
                {
                    rotation = 180;
                }
                else if (Input.GetKey(GameManager.keybind["ShootDown"]))
                {
                    rotation = 0;
                }
                else if (Input.GetKey(GameManager.keybind["ShootLeft"]))
                {
                    rotation = 270;
                }
                else if (Input.GetKey(GameManager.keybind["ShootRight"]))
                {
                    rotation = 90;
                }
                break;
        }

        head.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void Keypress()
    {
        Vector2 temp = Vector2.zero;

        if (Input.GetKey(GameManager.keybind["MoveLeft"]))
        {
            temp.x = -1;
        }
        else if (Input.GetKey(GameManager.keybind["MoveRight"]))
        {
            temp.x = 1;
        }
        else
        {
            temp.x = 0;
        }

        if (Input.GetKey(GameManager.keybind["MoveUp"]))
        {
            temp.y = 1;
        }
        else if (Input.GetKey(GameManager.keybind["MoveDown"]))
        {
            temp.y = -1;
        }
        else
        {
            temp.y = 0;
        }

        if (temp.y == 0 && temp.y == 0 && input.x != 0 || input.y != 0)
        {
            lastInput = input;
        }

        input = temp;
    }

    public void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        rigid.velocity = (input * slowApplied) * speed;

        AnimationController();
    }

    public void AnimationController()
    {
        anim.SetFloat("FacingX", lastInput.x);
        anim.SetFloat("FacingY", lastInput.y);

        if (input != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void TransitionCall(Vector3 direction)
    {
        lerpPosition = transform.position + (direction * 3);
        doorActive = true;
        Debug.Log("Cool");
    }

    public void LerpTransition()
    {
        if (transform.position != lerpPosition)
        {
            Vector3.Lerp(transform.position, lerpPosition, 100 * Time.deltaTime);
            Debug.Log("What?");
        }
        else
        {
            doorActive = false;
        }
    }
}

public enum InteractState
{
    Idle,
    Slowed,
    Occupied
}