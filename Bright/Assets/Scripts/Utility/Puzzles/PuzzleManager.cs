using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleType puzzleType;
    public List<PuzzleObjective> objectives;
    public GameObject puzzleStart;
    public Vector2Int puzzleSize;
    public int objectiveSize;
    public PuzzleState puzzleState = PuzzleState.Idle;

    public Transform puzzleParent;
    public BoxCollider2D col;
    public GameObject perk;

    public void Start()
    {
        PuzzleStart();
    }

    public void PuzzleStart()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
        col.size = puzzleSize;

        GetPuzzleList();
    }

    public void GetPuzzleList()
    {
        PuzzleObjective[] objectivesToAdd = puzzleParent.GetComponentsInChildren<PuzzleObjective>();

        for (int i = 0; i < objectivesToAdd.Length; i++)
        {
            objectives.Add(objectivesToAdd[i]);
        }

        puzzleStart = objectives[0].gameObject;
    }

    public void CheckComplete()
    {
        switch (puzzleType)
        {
            case PuzzleType.Memory:
                int counter = 0;

                for (int i = 0; i < objectives.Count; i++)
                {
                    if (objectives[i].activateState == ActivateState.Active)
                    {
                        counter++;
                    }
                }

                if (counter == objectives.Count)
                {
                    SetComplete();
                }
                break;
        }
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].Deactivate();
        }
    }

    public void SetComplete()
    {
        puzzleState = PuzzleState.Complete;

        col.enabled = false;

        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].col.enabled = false;
        }

        RewardDrop();
    }

    public void RewardDrop()
    {
        Instantiate(perk, new Vector2(0, 0), Quaternion.identity);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        switch (puzzleState)
        {
            case PuzzleState.Started:
                ResetPuzzle();
                break;
        }
    }
}

public enum PuzzleType
{
    PressurePlate,
    Lever,
    Memory,
}

public enum PuzzleState
{ 
    Idle,
    Started,
    Complete
}