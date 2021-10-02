using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueWorldGeneration : MonoBehaviour
{
    public struct WorldData
    {
        public Transform roomLocation;
        public Vector2Int roomPos;

        public WorldData(Transform location, Vector2Int pos)
        {
            roomLocation = location;
            roomPos = pos;
        }
    }

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

    public List<WorldData> GetDungeonLayout()
    {
        List<WorldData> temp = new List<WorldData>();

        for (int x = 0; x < dungeonGeneration.roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < dungeonGeneration.roomLayout.GetLength(1); y++)
            {
                if (dungeonGeneration.roomLayout[x, y] != null)
                {
                    Transform location = dungeonGeneration.roomLayout[x, y].room.transform;
                    Vector2Int selectedRoom = new Vector2Int(x, y);

                    temp.Add(new WorldData(location, selectedRoom));
                }
            }
        }

        return temp;
    }

    public void BuildTrueWorld()
    {
        List<WorldData> generatedFloor = GetDungeonLayout();

        for (int i = 0; i < generatedFloor.Count; i++)
        {
            string path = varietyPath + "Rooms/Variant_1";
            Vector2Int selectedRoom = generatedFloor[i].roomPos;

            dungeonGeneration.roomLayout[selectedRoom.x, selectedRoom.y].trueRoom = Instantiate(Resources.Load(path) as GameObject, generatedFloor[i].roomLocation.position, Quaternion.identity, trueWorldParent);
        }
    }
}