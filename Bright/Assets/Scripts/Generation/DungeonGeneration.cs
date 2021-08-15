using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public int roomMaxWidth;
    public int roomMaxHeight;
    public Vector2Int roomRange;
    public int maxRoomNumber;
    private int numberOfRooms;
    public string[,] roomLayout;
    public List<Vector2Int> generateRoomCoords = new List<Vector2Int>();
    private Vector2Int startRoom;
    public Vector2Int roomDimensions;
    public Transform floorParent;

    public float generationChance;
    public int generateOffset;


    public Dictionary<string, int> directions = new Dictionary<string, int>
    {
        { "Left", -1 },
        { "Right", 1 },
        { "Up", 1 },
        { "Down", -1 },
    };

    public int FloorSize(Vector2Int maxRooms)
    {
        return Random.Range(maxRooms.x, maxRooms.y);
    }

    public void Start()
    {
        GenerationCall();
    }

    public void GenerationCall()
    {
        maxRoomNumber = FloorSize(roomRange);
        GenerationStart();
    }

    public void GenerationStart()
    {
        numberOfRooms = maxRoomNumber;

        startRoom = new Vector2Int(roomMaxWidth / 2, roomMaxWidth / 2);

        roomLayout = new string[roomMaxWidth, roomMaxHeight];
        roomLayout[startRoom.x, startRoom.y] = "S";

        GenerateRoomLayout();

        switch (GeneratedCorrectly())
        {
            case false:
                Debug.Log("Generation failed, re-starting!" + System.Environment.NewLine + "Number of Rooms Left: " + numberOfRooms);
                generateRoomCoords.Clear();
                GenerationStart();
                break;
            case true:
                BuildRoomLayout(); //Move to GameManager or Level script later
                break;
        }
    }

    public void GenerateRoomLayout()
    {
        generateRoomCoords.Add(new Vector2Int(startRoom.x, startRoom.y));

        for (int i = 0; i < generateRoomCoords.Count; i++)
        {
            GenerateAjacentRooms(generateRoomCoords[i]);
        }

        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                if (roomLayout[x, y] == null)
                {
                    roomLayout[x, y] = "X";
                }
            }
        }
        MatrixDebug.CheckMatrix(roomLayout);
    }

    public void GenerateAjacentRooms(Vector2Int selectedRoom)
    {
        if (numberOfRooms > 0)
        {
            List<Vector2Int> sidesToAdd = AddSides(selectedRoom);

            for (int i = 0; i < sidesToAdd.Count; i++)
            {
                if (CheckChance(generationChance) && numberOfRooms > 0)
                {
                    roomLayout[sidesToAdd[i].x, sidesToAdd[i].y] = "R";
                    generateRoomCoords.Add(new Vector2Int(sidesToAdd[i].x, sidesToAdd[i].y));
                    numberOfRooms--;
                }
            }
        }
        else
        {
            return;
        }
    }

    public List<Vector2Int> AddSides(Vector2Int selectedRoom)
    {
        List<Vector2Int> temp = new List<Vector2Int>();
        List<Vector2Int> avaliableSides = CheckSides(selectedRoom);

        for (int i = 0; i < avaliableSides.Count; i++)
        {
            if (CheckNeighbour(avaliableSides[i]))
            {
                temp.Add(avaliableSides[i]);
            }
        }
        
        return temp;
    }

    public List<Vector2Int> CheckSides(Vector2Int selectedRoom)
    {
        List<Vector2Int> temp = new List<Vector2Int>();

        if (roomLayout[selectedRoom.x + directions["Left"], selectedRoom.y] == null)
        {
            temp.Add(new Vector2Int(selectedRoom.x + directions["Left"], selectedRoom.y));
        }
        if (roomLayout[selectedRoom.x + directions["Right"], selectedRoom.y] == null)
        {
            temp.Add(new Vector2Int(selectedRoom.x + directions["Right"], selectedRoom.y));
        }
        if (roomLayout[selectedRoom.x, selectedRoom.y + directions["Up"]] == null)
        {
            temp.Add(new Vector2Int(selectedRoom.x, selectedRoom.y + directions["Up"]));
        }
        if (roomLayout[selectedRoom.x, selectedRoom.y + directions["Down"]] == null)
        {
            temp.Add(new Vector2Int(selectedRoom.x, selectedRoom.y + directions["Down"]));
        }

        return temp;
    }

    public bool CheckNeighbour(Vector2Int neighbourRoom)
    {
        int counter = 0;

        if (roomLayout[neighbourRoom.x + directions["Left"], neighbourRoom.y] != null)
        {
            counter++;
        }
        if (roomLayout[neighbourRoom.x + directions["Right"], neighbourRoom.y] != null)
        {
            counter++;
        }
        if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Up"]] != null)
        {
            counter++;
        }
        if (roomLayout[neighbourRoom.x, neighbourRoom.y + directions["Down"]] != null)
        {
            counter++;
        }

        if (counter > 1 || counter == 0)
        {
            return false;
        }
        else
        {
            return true;
        }    
    }

    public bool CheckChance(float expected)
    {
        float chance = Random.Range(1, 11);

        if (chance < expected)
        {
            return true;
        }

        return false;
    }

    public bool GeneratedCorrectly()
    {
        if (numberOfRooms > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void BuildRoomLayout()
    {
        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                switch (roomLayout[y, x])
                {
                    case "S":
                        Instantiate(Resources.Load("Prefabs/Generation/Start") as GameObject, Vector2.zero, Quaternion.identity, floorParent);
                        break;
                    case "R":
                        Instantiate(Resources.Load("Prefabs/Generation/Room") as GameObject, new Vector2((x - generateOffset) * roomDimensions.x, (-y + generateOffset) * roomDimensions.y), Quaternion.identity, floorParent);
                        break;
                }
            }
        }
    }
}