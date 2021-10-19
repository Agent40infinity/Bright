using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerFirst : MonoBehaviour
{
    public SpawnerState spawnerState = SpawnerState.Idle;
    public List<GameObject> activeEnemies = new List<GameObject>();

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

    public List<GameObject> spawnpoints = new List<GameObject>();

    public Room room;
    public DungeonGeneration generation;

    public void Start()
    {
        generation = GameObject.FindWithTag("DungeonGeneration").GetComponent<DungeonGeneration>();

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
        room.attachedDoors[0].UpdateDoor(true);
        for (int i = 0; i < room.attachedDoors.Count; i++)
        {
            room.attachedDoors[i].UpdateDoor(true);
        }

        maxWeight = CalculateMaxWeight();
        List<EnemyType> enemiesToSpawn = GenerateEnemies();
        SpawnEnemies(new List<EnemyType>() { EnemyType.Wasp, EnemyType.Mushroom });
    }

    public int CalculateMaxWeight()
    {
        int temp = spawnpoints.Count * room.difficulty;

        return temp;
    }

    public List<EnemyType> GenerateEnemies()
    {
        List<EnemyType> temp = new List<EnemyType>();

        return temp;
    }

    public void SpawnEnemies(List<EnemyType> enemyList)
    {
        int index = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            activeEnemies.Add(Instantiate(enemyParent, spawnpoints[index].transform));
            activeEnemies[i].GetComponent<Enemy>().EnemySetup(enemyList[i]);

            if (index == spawnpoints.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }
}