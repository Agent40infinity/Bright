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

                    List<string> wallsToAdd = CheckNeighbour(selectedRoom, FillType.Wall);
                    AddWalls(selectedRoom, wallsToAdd, FillType.Wall);

                    List<string> doorsToAdd = CheckNeighbour(selectedRoom, FillType.Door);
                    AddWalls(selectedRoom, doorsToAdd, FillType.Door);
                }
            }
        }
    }

    public List<string> CheckNeighbour(Vector2Int neighbourRoom, FillType type)
    {
        List<string> temp = new List<string>();

        switch (type)
        {
            case FillType.Wall:
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
                break;
            case FillType.Door:
                if (roomLayout[neighbourRoom.x + directions["Left"], neighbourRoom.y] != null)
                {
                    temp.Add("Left");
                }
                if (roomLayout[neighbourRoom.x + directions["Right"], neighbourRoom.y] != null)
                {
                    temp.Add("Right");
                }
                if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Up"]] != null)
                {
                    temp.Add("Up");
                }
                if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Down"]] != null)
                {
                    temp.Add("Down");
                }
                break;
        }

        return temp;
    }

    public string DetermineType(int variant, int index)
    {
        if (forestVariants.ContainsKey(variant))
        {
            if (forestVariants[variant][index])
            {
                return "Forest/";
            }
        }

        return "Normal/";
    }

    public string DetermineType(string type, int index)
    {
        int variant = 0;

        switch (type)
        {
            case "S":
                variant = -1;
                break;
        }

        if (forestVariants.ContainsKey(variant))
        {
            if (forestVariants[variant][index])
            {
                return "Forest/";
            }
        }

        return "Normal/";
    }

    public void AddWalls(Vector2Int selectedRoom, List<string> sidesToAdd, FillType type)
    {
        Transform room = roomLayout[selectedRoom.x, selectedRoom.y].room.transform;
        int roomVariant = roomLayout[selectedRoom.x, selectedRoom.y].variant;

        for (int i = 0; i < sidesToAdd.Count; i++)
        {
            string roomType = "";
            int index = 0;

            switch (sidesToAdd[i])
            {
                case "Left":
                    index = 0;
                    break;
                case "Right":
                    index = 1;
                    break;
                case "Up":
                    index = 2;
                    break;
                case "Down":
                    index = 3;
                    break;
            }

            if (roomVariant != 0)
            {
                roomType = DetermineType(roomVariant, index);
            }
            else
            {
                roomType = DetermineType(roomLayout[selectedRoom.x, selectedRoom.y].type, index);
            }

            string path = fillPath + roomType + type.ToString() + "_" + sidesToAdd[i];

            GameObject fill = null;

            switch (sidesToAdd[i])
            {
                case "Left":
                    fill = Instantiate(Resources.Load(path) as GameObject, new Vector2((selectedRoom.y - generateOffset) * roomDimensions.x, ((-selectedRoom.x + generateOffset) * roomDimensions.y) + roomDimensions.y / 2 - 0.5f), Quaternion.identity, room);
                    break;
                case "Right":
                    fill = Instantiate(Resources.Load(path) as GameObject, new Vector2((selectedRoom.y - generateOffset) * roomDimensions.x, ((-selectedRoom.x + generateOffset) * roomDimensions.y) - roomDimensions.y / 2 - 0.5f), Quaternion.identity, room);
                    break;
                case "Up":
                    fill = Instantiate(Resources.Load(path) as GameObject, new Vector2(((selectedRoom.y - generateOffset) * roomDimensions.x) + roomDimensions.x / 2, (-selectedRoom.x + generateOffset) * roomDimensions.y), Quaternion.identity, room);
                    break;
                case "Down":
                    fill = Instantiate(Resources.Load(path) as GameObject, new Vector2(((selectedRoom.y - generateOffset) * roomDimensions.x) - roomDimensions.x / 2, (-selectedRoom.x + generateOffset) * roomDimensions.y), Quaternion.identity, room);
                    break;
            }

            if (fill.GetComponentInChildren<Door>())
            {
                roomLayout[selectedRoom.x, selectedRoom.y].attachedDoors.Add(fill.GetComponentInChildren<Door>());
            }
        }
    }
}

public enum FillType
{ 
    Door,
    Wall
}