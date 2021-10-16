using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SpawnerState spawnerState = SpawnerState.Idle;

    public Room room;
    public DungeonGeneration generation;

    public void Start()
    {
        generation = GameObject.FindWithTag("DungeonGeneration").GetComponent<DungeonGeneration>();

        room = generation.roomLayout[GameManager.currentRoom.x, GameManager.currentRoom.y];
    }

    public void GetEnemies()
    { 
        
    }

    public void SpawnEnemies()
    { 
        
    }
}

public enum SpawnerState
{ 
    Idle,
    Active,
    Cleared
}