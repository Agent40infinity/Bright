using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjective : MonoBehaviour
{
    public ActivateState activateState = ActivateState.Inactive;
    public List<Sprite> sprites = new List<Sprite>();

    public PuzzleManager manager;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D col;

    public void Start()
    {
        manager = gameObject.GetComponentInParent<PuzzleManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    public void Activate()
    {
        switch (activateState)
        {
            case ActivateState.Active:
                manager.ResetPuzzle();
                break;
            case ActivateState.Inactive:
                activateState = ActivateState.Active;
                spriteRenderer.sprite = sprites[0];

                manager.CheckComplete();
                break;
        }
    }

    public void Deactivate()
    {
        activateState = ActivateState.Inactive;
        spriteRenderer.sprite = sprites[1];
    }
        
    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                switch (manager.puzzleState)
                {
                    case PuzzleState.Idle:
                        if (gameObject == manager.puzzleStart)
                        {
                            Activate();
                            manager.puzzleState = PuzzleState.Started;
                        }
                        break;
                    case PuzzleState.Started:
                        Activate();
                        break;
                } 
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
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