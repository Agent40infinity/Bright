using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonWallGenerate : DungeonLayout
{
    public void WallGenerationCall()
    {
        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                if (roomLayout[x, y] != null)
                {
                    Vector2Int selectedRoom = new Vector2Int(x, y);

                    List<string> wallsToAdd = CheckNeighbour(selectedRoom);
                    AddWalls(selectedRoom, wallsToAdd);
                }
            }
        }
    }

    public List<string> CheckNeighbour(Vector2Int neighbourRoom)
    {
        List<string> temp = new List<string>();

        if (roomLayout[neighbourRoom.x + directions["Left"], neighbourRoom.y] == null)
        {
            temp.Add("Left");
        }
        if (roomLayout[neighbourRoom.x + directions["Right"], neighbourRoom.y] == null)
        {
            temp.Add("Right");
        }
        if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Up"]] == null)
        {
            temp.Add("Up");
        }
        if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Down"]] == null)
        {
            temp.Add("Down");
        }

        return temp;
    }

    public void AddWalls(Vector2Int selectedRoom, List<string> wallsToAdd)
    {
        for (int i = 0; i < wallsToAdd.Count; i++)
        {
            switch (wallsToAdd[i])
            {
                case "Left":
                    Instantiate(Resources.Load(wallPath[0]) as GameObject, new Vector2((selectedRoom.y - generateOffset) * roomDimensions.x, ((-selectedRoom.x + generateOffset) * roomDimensions.y) + roomDimensions.y / 2 - 0.5f), Quaternion.identity, floorParent);
                    break;
                case "Right":
                    Instantiate(Resources.Load(wallPath[0]) as GameObject, new Vector2((selectedRoom.y - generateOffset) * roomDimensions.x, ((-selectedRoom.x + generateOffset) * roomDimensions.y) - roomDimensions.y / 2 - 0.5f), Quaternion.identity, floorParent);
                    break;
                case "Up":
                    Instantiate(Resources.Load(wallPath[1]) as GameObject, new Vector2(((selectedRoom.y - generateOffset) * roomDimensions.x) + roomDimensions.x / 2, (-selectedRoom.x + generateOffset) * roomDimensions.y), Quaternion.identity, floorParent);
                    break;
                case "Down":
                    Instantiate(Resources.Load(wallPath[1]) as GameObject, new Vector2(((selectedRoom.y - generateOffset) * roomDimensions.x) - roomDimensions.x / 2, (-selectedRoom.x + generateOffset) * roomDimensions.y), Quaternion.identity, floorParent);
                    break;
            }
        }
    }
}