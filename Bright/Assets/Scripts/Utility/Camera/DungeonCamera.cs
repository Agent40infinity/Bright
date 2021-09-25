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

    public void OcclusionCulling(Vector2Int previousRoom)
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
                        generation.roomLayout[GameManager.currentRoom.x, GameManager.currentRoom.y].room.SetActive(true);
                    }
                    else if (room != previousRoom)
                    {
                        generation.roomLayout[i, j].room.SetActive(false);
                    }
                }
            }
        }

        StartCoroutine(FadePrevious(previousRoom));
    }

    public IEnumerator FadePrevious(Vector2Int previousRoom)
    {
        yield return new WaitForSeconds(1);
        generation.roomLayout[previousRoom.x, previousRoom.y].room.SetActive(false)
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
                        Vector2Int previousRoom = GameManager.currentRoom;

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
                        OcclusionCulling(previousRoom);
                        break;
                    case FollowState.Follow:

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