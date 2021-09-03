using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : DungeonWallGenerate
{
    public void Awake()
    {
        GenerationCall();
        WallGenerationCall();
        GameObject.FindWithTag("MainCamera").GetComponent<DungeonCamera>().generation = this;
        GameManager.dungeonRef = gameObject;
        ContentStart();
    }

    public void ContentStart()
    {
        AddContent();
    }

    public string GetDifficultyVariant(int difficultyIndex)
    {
        int chance = Random.Range(0, difficultiesLeft[difficultyIndex].Count);
        int temp = difficultiesLeft[difficultyIndex][chance];
        difficultiesLeft[difficultyIndex].RemoveAt(chance);

        return "_" + temp;
    }

    public void AddContent()
    {
        for (int x = 0; x < roomLayout.GetLength(0); x++)
        {
            for (int y = 0; y < roomLayout.GetLength(1); y++)
            {
                if (roomLayout[y, x] != null && roomLayout[y, x].difficulty != 0)
                {
                    int index = roomLayout[y, x].difficulty;
                    string path = difficultyPath + "Difficulty_" + index + GetDifficultyVariant(index);

                    Instantiate(Resources.Load(path) as GameObject, new Vector2((x - generateOffset) * roomDimensions.x, (-y + generateOffset) * roomDimensions.y), Quaternion.identity, floorParent);
                }
            }
        }
    }
}
