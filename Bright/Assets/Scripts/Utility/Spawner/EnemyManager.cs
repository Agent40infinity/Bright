using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SpawnerState spawnerState = SpawnerState.Idle;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public int activeIndex;

    public List<Vector2Int> activeCoords = new List<Vector2Int>();

    public GameObject enemyParent;
    public Dictionary<EnemyType, int> weighting = new Dictionary<EnemyType, int>
    {
        { EnemyType.Wasp, 1 },
        { EnemyType.Mushroom, 1 },
        { EnemyType.Flower, 2 },
        { EnemyType.SeedBomb, 2 },
        { EnemyType.Sunflower, 3 },
    };
    public int maxWeight;

    public Room room;
    public DungeonGeneration generation;
    public PathfindingGrid pathfinding;

    public void Start()
    {
        generation = GameObject.FindWithTag("DungeonGeneration").GetComponent<DungeonGeneration>();
        pathfinding = GameObject.FindWithTag("Pathfinding").GetComponent<PathfindingGrid>();

        room = generation.roomLayout[GameManager.currentRoom.x, GameManager.currentRoom.y];
    }

    public void Update()
    { 
        switch (spawnerState)
        {
            case SpawnerState.Idle:
                SetupSpawn();
                spawnerState = SpawnerState.Active;
                break;
        }
    }

    public void SetupSpawn()
    {
        DoorUpdate();
        maxWeight = CalculateDifficulty();

        List<EnemyType> enemiesToSpawn = GenerateEnemies();

        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            SpawnEnemy(enemiesToSpawn[i], GenerateSpawnLocation());
        }
    }

    public void DoorUpdate()
    {
        room.attachedDoors[0].UpdateDoor(true);
        for (int i = 0; i < room.attachedDoors.Count; i++)
        {
            room.attachedDoors[i].UpdateDoor(true);
        }
    }

    public int CalculateDifficulty()
    {
        int temp;
        int difficulty = room.difficulty;

        switch (difficulty)
        {
            case 1:
                difficulty *= 2;
                break;
            case 2: case 3:
                difficulty += 1;
                break;
        }

        temp = Random.Range(difficulty, difficulty * 2);

        return temp;
    }

    public List<EnemyType> GenerateEnemies()
    {
        List<EnemyType> temps = new List<EnemyType>() { EnemyType.Mushroom, EnemyType.Mushroom, EnemyType.Mushroom, EnemyType.Wasp };

        return temp;
    }

    public Vector2 GenerateSpawnLocation()
    {
        Vector2 temp = new Vector2();

        pathfinding.grid;

        return temp;
    }

    public void SpawnEnemy(EnemyType enemyList, Vector2 spawnpoint)
    {
        activeEnemies.Add(Instantiate(enemyParent, spawnpoint, Quaternion.identity, room.room.transform));
        activeEnemies[activeIndex].GetComponent<Enemy>().EnemySetup(enemyList);
        activeIndex++;
    }
}

public enum SpawnerState
{ 
    Idle,
    Active,
    Cleared
}