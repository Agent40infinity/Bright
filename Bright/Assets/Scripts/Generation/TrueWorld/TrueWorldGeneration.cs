using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueWorldGeneration : MonoBehaviour
{
    public string[,] trueRoomLayout;
    public DungeonGeneration dungeonGeneration;

    public Transform trueWorldParent;

    public string varietyPath = "Prefabs/Generation/True/";

    public void TrueGenerationCall()
    {
        GameManager.trueWorldRef = gameObject;
        TrueGenerationStart();
    }

    public void TrueGenerationStart()   
    {
        BuildTrueWorld();

        gameObject.SetActive(false);
    }

    public List<Transform> GetDungeonLayout()
    {
        List<Transform> temp = new List<Transform>();

        for (int x = 0; x < dungeonGeneration.roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < dungeonGeneration.roomLayout.GetLength(1); y++)
            {
                if (dungeonGeneration.roomLayout[x, y] != null)
                {
                    temp.Add(dungeonGeneration.roomLayout[x, y].room.transform);
                }
            }
        }

        return temp;
    }

    public void BuildTrueWorld()
    {
        List<Transform> generatedFloor = GetDungeonLayout();

        for (int i = 0; i < generatedFloor.Count; i++)
        {
            string path = varietyPath + "Rooms/Variant_1";

            Instantiate(Resources.Load(path), generatedFloor[i].position, Quaternion.identity, trueWorldParent);
        }
    }
}