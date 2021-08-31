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
        
    }
}
