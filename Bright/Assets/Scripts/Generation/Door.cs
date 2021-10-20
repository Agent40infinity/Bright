using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public LockState lockState;

    public Animator anim;
    public BoxCollider2D col;

    public void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    public void UpdateDoor(bool lockState)
    {
        switch (lockState)
        {
            case true:
                col.isTrigger = false;
                this.lockState = LockState.Locked;
                break;
            case false:
                col.isTrigger = true;
                this.lockState = LockState.Unlocked;
                break;

        }

        anim.SetBool("Locked", lockState);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                Vector3 direction = Vector3.zero;

                switch (transform.rotation.z)
                {
                    case 0:
                        direction = Vector2.up;
                        break;
                    case 90:
                        direction = Vector2.left;
                        break;
                    case 180:
                        direction = Vector2.down;
                        break;
                    case 270:
                        direction = Vector2.right;
                        break;
                }

                PlayerPhysics physics = other.GetComponent<PlayerPhysics>();
                physics.TransitionCall(direction);
                break;
        }
    }
}

public enum LockState
{ 
    Unlocked,
    Locked
}