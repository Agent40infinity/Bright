using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjective : MonoBehaviour
{
    public ActivateState activateState = ActivateState.Inactive;

    public PuzzleManager manager;
    public BoxCollider2D col;
    public Animator anim;

    public void Start()
    {
        manager = gameObject.GetComponentInParent<PuzzleManager>();
        col = gameObject.GetComponent<BoxCollider2D>();
        anim = gameObject.GetComponent<Animator>();

        if (gameObject == manager.puzzleStart)
        {
            anim.SetBool("Start", true);
        }
    }

    public void Activate()
    {
        switch (manager.puzzleState)
        {
            case PuzzleState.Idle:
                if (gameObject == manager.puzzleStart)
                {
                    manager.puzzleState = PuzzleState.Started;
                }
                else
                {
                    return;
                }
                break;
        }

        switch (activateState)
        {
            case ActivateState.Active:
                manager.ResetPuzzle();
                break;
            case ActivateState.Inactive:
                activateState = ActivateState.Active;
                anim.SetBool("Active", true);
                manager.CheckComplete();
                break;
        }
    }

    public void Deactivate()
    {
        activateState = ActivateState.Inactive;
        anim.SetTrigger("Failed");
        anim.SetBool("Active", false);
    }
        
    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PlayerGround":
                Activate();
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PlayerGround":
                switch (manager.puzzleType)
                {
                    case PuzzleType.PressurePlate:
                        Deactivate();
                        break;
                }
                break;
        }
    }
}

public enum ActivateState
{ 
    Active,
    Inactive
}