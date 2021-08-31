using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLayout1 : MonoBehaviour
{
    [Header("Room Attributes")]
    public int roomMaxWidth;
    public int roomMaxHeight;
    public Vector2Int roomRange;
    public int maxRoomNumber;
    public Vector2Int roomDimensions;

    [Header("Generation")]
    public float generationChance;
    public int generateOffset;
    bool[] generatedUniques = new bool[2];

    private int numberOfRooms;
    private Vector2Int startRoom;

    [Header("Layout")]
    public List<Vector2Int> generateRoomCoords = new List<Vector2Int>();
    public Room[,] roomLayout;

    public Dictionary<string, int> directions = new Dictionary<string, int>
    {
        { "Left", -1 },
        { "Right", 1 },
        { "Up", 1 },
        { "Down", -1 },
    };

    public Dictionary<int, float[]> difficultyChance = new Dictionary<int, float[]>
    {
        { 1, new float[] { 7, 1.5f, 0.7f, 0.5f, 0.3f} },
        { 2, new float[] { 2, 5, 1.5f, 1, 0.5f} },
        { 3, new float[] { 1, 2, 4, 2, 1} },
        { 4, new float[] { 0.5f, 0.5f, 1, 5, 3} },
        { 5, new float[] { 0.5f, 0.5f, 1, 3, 5} },
    };

    [Header("Reference")]
    public Transform floorParent;
    public TrueWorldGeneration trueWorldGeneration;
    private string path = "Prefabs/Generation/Normal/";
    public string[] wallPath = new string[] { "Prefabs/Generation/Wall_Horizontal", "Prefabs/Generation/Wall_Vertical" };

    public int FloorSize(Vector2Int maxRooms)
    {
        return Random.Range(maxRooms.x, maxRooms.y);
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

        roomLayout = new Room[roomMaxWidth, roomMaxHeight];
        CreateRoom(startRoom, "S", 0);

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
                trueWorldGeneration.TrueGenerationCall();
                break;
        }
    }

    public void CreateRoom(Vector2Int selectedRoom, string type, int varient)
    {
        //roomLayout[selectedRoom.x, selectedRoom.y] = new Room(type, varient);
    }

    public void GenerateRoomLayout()
    {
        generateRoomCoords.Add(new Vector2Int(startRoom.x, startRoom.y));

        for (int i = 0; i < generateRoomCoords.Count; i++)
        {
            GenerateAjacentRooms(generateRoomCoords[i]);
        }

        GenerateUniqueStart();

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
                    Vector2Int newRoom = new Vector2Int(sidesToAdd[i].x, sidesToAdd[i].y);
                    CreateRoom(newRoom, "R", Random.Range(1, 6));
                    GenerateRoomDifficulty(newRoom);
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
            if (CheckNeighbour(avaliableSides[i], 1))
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

    public bool CheckNeighbour(Vector2Int neighbourRoom, int neighbourMax)
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

        if (counter != neighbourMax || counter == 0)
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

    public void GenerateUniqueStart()
    {

        generatedUniques = new bool[] { false, false };

        for (int x = 1; x < roomLayout.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < roomLayout.GetLength(1) - 1; y++)
            {
                if (!generatedUniques[0] || !generatedUniques[1])
                {
                    if (roomLayout[x, y] == null)
                    {
                        Vector2Int selectedRoom = new Vector2Int(x, y);
                        switch (generatedUniques[0])
                        {
                            case false:
                                if (GenerateUniqueRoom(SpecialState.Boss, selectedRoom))
                                {
                                    generatedUniques[0] = true;
                                }
                                break;
                        }
                        switch (generatedUniques[1])
                        {
                            case false:
                                if (GenerateUniqueRoom(SpecialState.Shop, selectedRoom))
                                {
                                    generatedUniques[1] = true;
                                }
                                break;
                        }
                    }
                }
                else if (generatedUniques[0] && generatedUniques[1])
                {
                    return;
                }
            }
        }
    }

    public bool GenerateUniqueRoom(SpecialState special, Vector2Int selectedRoom)
    {
        switch (special)
        {
            case SpecialState.Boss:
                if (CheckNeighbour(selectedRoom, 1) && CheckChance(2.5f))
                {
                    CreateRoom(selectedRoom, "B", 0);
                    return true;
                }
                else
                {
                    return false;
                }
            case SpecialState.Shop:
                if (CheckNeighbour(selectedRoom, 2) && CheckChance(2.5f))
                {
                    CreateRoom(selectedRoom, "C", 0);
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                Debug.Log("Return Error, Unique not found!");
                return false;
        }
    }

    public void GenerateRoomDifficulty(Vector2Int selectedRoom)
    {
        float roomsLeft = (float)numberOfRooms / maxRoomNumber;
        int difficultyIndex = DetermineDifficultyScale(roomsLeft);

        roomLayout[selectedRoom.x, selectedRoom.y].variant = DetermineDifficulty(difficultyChance[difficultyIndex]);
    }

    public int DetermineDifficultyScale(float roomsLeft)
    {
        if (roomsLeft <= 1 && roomsLeft > 0.8)
        {
            return 1;
        }
        else if (roomsLeft <= 0.8 && roomsLeft > 0.6)
        {
            return 2;
        }
        else if (roomsLeft <= 0.6 && roomsLeft > 0.4)
        {
            return 3;
        }
        else if (roomsLeft <= 0.4 && roomsLeft > 0.2)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    public int DetermineDifficulty(float[] chance)
    {
        float selection = Random.Range(0, 11);

        if (selection <= AddChance(0, chance))
        {
            return 1;
        }
        else if (selection <= AddChance(1, chance) && selection > AddChance(0, chance))
        {
            return 2;
        }
        else if (selection <= AddChance(2, chance) && selection > AddChance(1, chance))
        {
            return 3;
        }
        else if (selection <= AddChance(3, chance) && selection > AddChance(2, chance))
        {
            return 4;
        }
        else 
        {
            return 5;
        }   
    }

    public float AddChance(int index, float[] chance)
    {
        float temp = 0;

        for (int i = 0; i < index + 1; i++)
        {
            temp += chance[i];
        }

        Debug.Log(temp);
        return temp;
    }

    public bool GeneratedCorrectly()
    {
        if (numberOfRooms == 0 && generatedUniques[0] && generatedUniques[1])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BuildRoomLayout()
    {
        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                if (roomLayout[y, x] != null)
                {
                    string varientPath = "";

                    switch (roomLayout[y, x].type)
                    {
                        case "S":
                            varientPath = path + "Start";
                            break;
                        case "B":
                            varientPath = path + "Boss";
                            break;
                        case "C":
                            varientPath = path + "Shop";
                            break;
                        case "R":
                            varientPath = path + "Rooms/Variant_" + roomLayout[y, x].variant;
                            break;
                    }

                    Instantiate(Resources.Load(varientPath) as GameObject, new Vector2((x - generateOffset) * roomDimensions.x, (-y + generateOffset) * roomDimensions.y), Quaternion.identity, floorParent);
                }
            }
        }
    }
}