using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public int roomMaxWidth;
    public int roomMaxHeight;
    public int maxRoomNumber;
    private int numberOfRooms;
    public string[,] roomLayout;
    public List<int[]> generateRoomCoords = new List<int[]>();
    private int[] startRoom = new int[2];

    public Dictionary<string, int> directions = new Dictionary<string, int>
    {
        { "Left", -1 },
        { "Right", 1 },
        { "Up", 1 },
        { "Down", -1 },
    };

    private float[] generationChance = { 2.5f, 5, 7.5f, 10};

    //Move later
    public List<GameObject> levels = new List<GameObject>();
    public string[,] layout;
    public int selectedLevel;

    public void Start()
    {
        GenerationCall();
    }

    public void GenerationCall()
    {
        GenerationStart(); //levels[selectedLevel]
    }

    public void GenerationStart()
    {
        numberOfRooms = maxRoomNumber;
        startRoom[0] = roomMaxWidth / 2;
        startRoom[1] = roomMaxWidth / 2;

        roomLayout = new string[roomMaxWidth, roomMaxHeight];
        roomLayout[startRoom[0], startRoom[1]] = "S";
        
        int[] selectedRoom = {startRoom[0], startRoom[1]};

        GenerateAjacentRooms(selectedRoom);

        for (int i = 0; i < generateRoomCoords.Count; i++)
        {
            selectedRoom = generateRoomCoords[i];
            if (numberOfRooms >= 0)
            {
                GenerateAjacentRooms(selectedRoom);
            }
        }
    }

    public void GenerateAjacentRooms(int[] selectedRoom)
    {
        List<string> possibleSides = CheckSides(selectedRoom);

        List<string> generationOrder = GetSideOrder(possibleSides);

        for (int i = generationOrder.Count - 1; i >= 0; i--)
        {
            if (CheckChance(generationChance[i]))
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
                        generateRoomCoords.Add(new int[] { selectedRoom[0] + directions["Right"], selectedRoom[1]});
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

        MatrixDebug.CheckMatrix(roomLayout);
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
}