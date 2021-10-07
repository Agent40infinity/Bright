    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    [Header("Camera")]
    public Vector3 cameraPos;
    public Vector3 resetPos;
    public FollowState followState = FollowState.Cell;
    public MoveState moveState = MoveState.Active;

    [Header("Cell Mode")]   
    public float switchSpeed;

    [Header("Reference")]
    public DungeonGeneration generation;
    public PathfindingGrid pathfinding;

    public void PathfindingCall()
    {
    
        {
            pathfinding = GameObject.FindWithTag("Pathfinding").GetComponent<PathfindingGrid>();
        }

        pathfinding.BindCamera(transform);
    }

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
            pathfinding.DrawGrid();
        }
    }

    public void OcclusionCulling()
    {
        for (int i = 0; i < generation.roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < generation.roomLayout.GetLength(1); j++)
            {
                if (generation.roomLayout[i, j] != null)
                {
                    Vector2Int room = new Vector2Int(i, j);

                    if (room == GameManager.currentRoom)
                    {
                        RoomStatus(room, true);
                    }
                    else
                    {
                        RoomStatus(room, false);
                    }
                }
            }
        }
    }

    public void RoomStatus(Vector2Int selectedRoom, bool active)
    {
        generation.roomLayout[selectedRoom.x, selectedRoom.y].room.SetActive(active);
        generation.roomLayout[selectedRoom.x, selectedRoom.y].trueRoom.SetActive(active);
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
                            GameManager.currentRoom.y++;
                        }
                        else if (position.x < cameraPos.x && (position.y < cameraPos.y + generation.roomDimensions.y / 2 && position.y > cameraPos.y - generation.roomDimensions.y / 2))
                        {
                            cameraPos.x -= generation.roomDimensions.x;
                            GameManager.currentRoom.y--;
                        }
                        else if (position.y > cameraPos.y && (position.x < cameraPos.x + generation.roomDimensions.x / 2 && position.x > cameraPos.x - generation.roomDimensions.x / 2))
                        {
                            cameraPos.y += generation.roomDimensions.y;
                            GameManager.currentRoom.x--;
                        }
                        else if (position.y < cameraPos.y && (position.x < cameraPos.x + generation.roomDimensions.x / 2 && position.x > cameraPos.x - generation.roomDimensions.x / 2))
                        {
                            cameraPos.y -= generation.roomDimensions.y;
                            GameManager.currentRoom.x++;
                        }

                        moveState = MoveState.Active;
                        OcclusionCulling();
                        break;
                }
                break;
        }
    }

    public void ResetCamera()
    {
        cameraPos = resetPos;
        transform.position = resetPos;
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