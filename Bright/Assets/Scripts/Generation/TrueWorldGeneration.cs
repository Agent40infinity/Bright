using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueWorldGeneration : MonoBehaviour
{
    public string[,] trueRoomLayout;
    public DungeonGeneration dungeonGeneration;

    public GameObject dungeon;
    public Transform trueWorldParent;

    public void TrueGenerationCall()
    {
        GameManager.trueWorldRef = gameObject;
        TrueGenerationStart();
    }

    public void TrueGenerationStart()   
    {
        trueRoomLayout = dungeonGeneration.roomLayout;

        BuildTrueWorld();

        gameObject.SetActive(false);
    }

    public void BuildTrueWorld()
    {
        List<Transform> generatedFloor = new List<Transform>(dungeon.GetComponentsInChildren<Transform>());

        for (int i = 1; i < generatedFloor.Count; i++)
        {
            switch (generatedFloor[i].name.Contains("Room"))
            {
                case true:
                    Instantiate(generatedFloor[i], trueWorldParent);
                    break;
            }
        }
    }
}
