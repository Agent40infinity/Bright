using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration1 : MonoBehaviour
{
    public int roomMaxWidth;
    public int roomMaxHeight;
    public int maxRoomNumber;
    private int numberOfRooms;
    public string[,] roomLayout;
    public List<int[]> generateRoomCoords = new List<int[]>();
    private int[] startRoom = new int[2];
    public Vector2 roomDimensions;
    public int generateOffset;

    public Dictionary<string, int> directions = new Dictionary<string, int>
    {
        { "Left", -1 },
        { "Right", 1 },
        { "Up", 1 },
        { "Down", -1 },
    };

    private float[] generationChance = { 2.5f, 5, 7.5f, 10};

    public void Start()
    {
        GenerationCall();
    }

    public void GenerationCall()
    {
        GenerationStart();
    }

    public void GenerationStart()
    {
        numberOfRooms = maxRoomNumber;
        startRoom[0] = roomMaxWidth / 2;
        startRoom[1] = roomMaxWidth / 2;

        roomLayout = new string[roomMaxWidth, roomMaxHeight];
        roomLayout[startRoom[0], startRoom[1]] = "S";

        GenerateRoomLayout();

        switch (GeneratedCorrectly())
        {
            case false:
                Debug.Log("Generation failed, re-starting!");
                GenerationStart();
                break;
            case true:
                BuildRoomLayout(); //Move to GameManager or Level script later
                break;
        }
    }

    public void GenerateRoomLayout()
    {
        int[] selectedRoom = { startRoom[0], startRoom[1] };

        GenerateAjacentRooms(selectedRoom);

        for (int i = 0; i < generateRoomCoords.Count; i++)
        {
            selectedRoom = generateRoomCoords[i];
            GenerateAjacentRooms(selectedRoom);
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
        //MatrixDebug.CheckMatrix(roomLayout);
    }

    public void GenerateAjacentRooms(int[] selectedRoom)
    {
        if (numberOfRooms > 0)
        {
            List<string> possibleSides = CheckSides(selectedRoom);

            List<string> generationOrder = GetSideOrder(possibleSides);

            for (int i = generationOrder.Count - 1; i >= 0; i--)
            {

                if (CheckChance(generationChance[i]) && numberOfRooms > 0)
                {
                    switch (generationOrder[i])
                    {
                        case "Left":
                            roomLayout[selectedRoom[0] + directions["Left"], selectedRoom[1]] = "R";
                            generateRoomCoords.Add(new int[] { selectedRoom[0] + directions["Left"], selectedRoom[1] });
                            numberOfRooms--;
                            break;
                        case "Right":
                            roomLayout[selectedRoom[0] + directions["Right"], selectedRoom[1]] = "R";
                            generateRoomCoords.Add(new int[] { selectedRoom[0] + directions["Right"], selectedRoom[1] });
                            numberOfRooms--;
                            break;
                        case "Up":
                            roomLayout[selectedRoom[0], selectedRoom[1] + directions["Up"]] = "R";
                            generateRoomCoords.Add(new int[] { selectedRoom[0], selectedRoom[1] + directions["Up"] });
                            numberOfRooms--;
                            break;
                        case "Down":
                            roomLayout[selectedRoom[0], selectedRoom[1] + directions["Down"]] = "R";
                            generateRoomCoords.Add(new int[] { selectedRoom[0], selectedRoom[1] + directions["Down"] });
                            numberOfRooms--;
                            break;
                        default:
                            Debug.Log("Error Log: Generation attempt failed");
                            return;
                    }
                }
            }
        }
        else
        {
            return;
        }       
    }

    public List<string> CheckSides(int[] selectedRoom)
    {
        List<string> temp = new List<string>();
        
        if (roomLayout[selectedRoom[0] + directions["Left"], selectedRoom[1]] == null)
        {
            temp.Add("Left");
        }
        if (roomLayout[selectedRoom[0] + directions["Right"], selectedRoom[1]] == null)
        {
            temp.Add("Right");
        }
        if (roomLayout[selectedRoom[0], selectedRoom[1] + directions["Up"]] == null)
        {
            temp.Add("Up");
        }
        if (roomLayout[selectedRoom[0], selectedRoom[1] + directions["Down"]] == null)
        {
            temp.Add("Down");
        }

        //for (int i = 0; i < temp.Count; i++)
        //{
        //    Debug.Log("CheckSides " + i + ": " + temp[i]);
        //}

        return temp;
    }

    public List<string> GetSideOrder(List<string> sides)
    {
        List<string> temp = new List<string>();

        for (int i = sides.Count - 1; i >= 0; i--)
        {
            int chance = Random.Range(0, sides.Count);
            temp.Add(sides[chance]);
            sides.RemoveAt(chance);
        }

        //for (int i = 0; i < temp.Count; i++)
        //{
        //    Debug.Log("GetSideOrder " + i + ": " + temp[i]);
        //}

        return temp;
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
        int roomCounter = 0;

        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                switch (roomLayout[x, y])
                {
                    case "R":
                        roomCounter++;
                        break;
                }
            }
        }

        if (roomCounter < maxRoomNumber)
        {
            return false;
        }

        return true;
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
                        Instantiate(Resources.Load("Prefabs/Utility/Start") as GameObject, Vector2.zero, Quaternion.identity);
                        break;
                    case "R":
                        Instantiate(Resources.Load("Prefabs/Utility/Room") as GameObject, new Vector2((x - generateOffset) * roomDimensions.x, (-y + generateOffset) * roomDimensions.y), Quaternion.identity);
                        break;
                }
            }
        }
    }
}