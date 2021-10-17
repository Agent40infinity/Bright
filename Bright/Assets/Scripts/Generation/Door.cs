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
}

public enum LockState
{ 
    Unlocked,
    Locked
}