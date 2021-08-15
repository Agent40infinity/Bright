    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    public Vector3 cameraPos;
    public float switchSpeed;

    public FollowState followState = FollowState.Cell;
    public MoveState moveState = MoveState.Active;

    public DungeonGeneration generation;

    public void Update()
    {
        switch (moveState)
        {
            case MoveState.Active:
                LerpCamera();
                break;
        }
    }

    public void LerpCamera()
    {
        if (transform.position != cameraPos)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPos, switchSpeed * Time.deltaTime);
        }
        else
        {
            moveState = MoveState.Inactive;
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                switch (followState)
                {
                    case FollowState.Cell:
                        Vector2 position = other.GetComponent<Transform>().position;

                        if (position.x > cameraPos.x && (position.y < cameraPos.y + generation.roomDimensions.y / 2 && position.y > cameraPos.y - generation.roomDimensions.y / 2))
                        {
                            cameraPos.x += generation.roomDimensions.x;
                        }
                        else if (position.x < cameraPos.x && (position.y < cameraPos.y + generation.roomDimensions.y / 2 && position.y > cameraPos.y - generation.roomDimensions.y / 2))
                        {
                            cameraPos.x -= generation.roomDimensions.x;
                        }
                        else if (position.y > cameraPos.y && (position.x < cameraPos.x + generation.roomDimensions.x / 2 && position.x > cameraPos.x - generation.roomDimensions.x / 2))
                        {
                            cameraPos.y += generation.roomDimensions.y;
                        }
                        else if (position.y < cameraPos.y && (position.x < cameraPos.x + generation.roomDimensions.x / 2 && position.x > cameraPos.x - generation.roomDimensions.x / 2))
                        {
                            cameraPos.y -= generation.roomDimensions.y;
                        }

                        moveState = MoveState.Active;
                        break;
                    case FollowState.Follow:

                        break;
                }
                break;
        }
    }
}

public enum FollowState
{ 
    Cell, 
    Follow,
    Idle
}

public enum MoveState
{ 
    Active,
    Inactive
}
