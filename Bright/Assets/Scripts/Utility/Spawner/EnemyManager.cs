using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SpawnerState spawnerState = SpawnerState.Idle;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public int activeIndex;
    public int divider;

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
    public bool fullStack = false;

    public Room room;
    public DungeonGeneration generation;
    public PathfindingGrid pathfinding;

    public int RandomChance()
    {
        return Random.Range(1, 11);
    }

    public void Start()
    {
        generation = GameObject.FindWithTag("DungeonGeneration").GetComponent<DungeonGeneration>();
        pathfinding = GameObject.FindWithTag("Pathfinding").GetComponent<PathfindingGrid>();

        room = generation.roomLayout[GameManager.currentRoom.x, GameManager.currentRoom.y];

        divider = pathfinding.grid.GetLength(0) / (int)pathfinding.gridSize.x;
    }

    public void Update()
    { 
        switch (spawnerState)
        {
            case SpawnerState.Idle:
                switch (pathfinding.gridState)
                {
                    case GridState.Moved:
                        pathfinding.gridState = GridState.Idle;
                        StartCoroutine("Awaken");
                        spawnerState = SpawnerState.Active;
                        break;
                }
                break;
            case SpawnerState.Active:
                CheckCompletion();
                break;
        }
    }

    public IEnumerator Awaken()
    {
        yield return new WaitForSeconds(0.1f);
        SetupSpawn();
    }

    public void SetupSpawn()
    {
        DoorUpdate(true);
        maxWeight = CalculateDifficulty();

        List<EnemyType> enemiesToSpawn = GenerateEnemies();

        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            SpawnEnemy(enemiesToSpawn[i], GenerateSpawnLocation());
        }
    }

    public void CheckCompletion()
    {
        switch (activeEnemies.Count)
        {
            case 0:
                DoorUpdate(false);
                spawnerState = SpawnerState.Cleared;
                break;
        }
    }    

    public void DoorUpdate(bool locked)
    {
        switch (locked)
        {
            case true:
                for (int i = 0; i < room.attachedDoors.Count; i++)
                {
                    room.attachedDoors[i].UpdateDoor(true);
                }
                break;
            case false:
                for (int i = 0; i < room.attachedDoors.Count; i++)
                {
                    room.attachedDoors[i].UpdateDoor(false);
                }
                break;
        }

    }

    public int CalculateDifficulty()
    {
        int temp;
        int difficulty = room.difficulty;

        switch (difficulty)
        {
            case 1: case 2: case 3:
                difficulty += 1;
                break;
        }

        temp = Random.Range(difficulty, difficulty * 2);

        return temp;
    }

    public List<EnemyType> GenerateEnemies()
    {
        List<EnemyType> temp = new List<EnemyType>();

        for (int i = 0; i < maxWeight; i++)
        {
            temp.Add(SelectEnemyType(temp, temp.Count - 1));
        }

        return temp;
    }

    public EnemyType SelectEnemyType(List<EnemyType> enemies, int index)
    {
        switch (fullStack)
        {
            case true:
                return enemies[index];
            case false:
                if (enemies.Count <= 0)
                {
                    int chance = Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length - 1);
                    int fullStack = RandomChance();
                    if (fullStack <= 2)
                    {
                        this.fullStack = true;
                    }

                    return (EnemyType)System.Enum.GetValues(typeof(EnemyType)).GetValue(chance);
                }
                else
                {
                    int duplicate = RandomChance();

                    if (duplicate > 5)
                    {
                        return enemies[index];
                    }

                    int chance = Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length - 1);
                    return (EnemyType)System.Enum.GetValues(typeof(EnemyType)).GetValue(chance);
                }
        }

        return (EnemyType)System.Enum.GetValues(typeof(EnemyType)).GetValue(0);
    }

    public Vector2 GenerateSpawnLocation()
    {
        Vector2Int selectedTile = Vector2Int.zero;

        while (selectedTile == Vector2Int.zero || !CheckValidity(selectedTile))
        {
            selectedTile = new Vector2Int(Random.Range(divider, pathfinding.grid.GetLength(0) - divider), Random.Range(divider , pathfinding.grid.GetLength(1) - divider));
        }

        //Debug.Log("Selected Tile: " + selectedTile + " | GridSize: " + pathfinding.grid.GetLength(0) + ", " + pathfinding.grid.GetLength(1));
        activeCoords.Add(selectedTile);
        return pathfinding.grid[selectedTile.x, selectedTile.y].worldPosition;
    }

    public bool CheckValidity(Vector2Int selected)
    {
        if (pathfinding.grid[selected.x, selected.y].walkable)
        {
            if (activeCoords.Count > 0)
            {
                Vector2Int posBounds = new Vector2Int(selected.x + divider, selected.y + divider);
                Vector2Int negBounds = new Vector2Int(selected.x - divider, selected.y - divider);
                int index = 0;

                for (int i = negBounds.x; i <= posBounds.x; i++)
                {
                    for (int j = negBounds.y; j < posBounds.y; j++)
                    {
                        Vector2Int current = new Vector2Int(i, j);

                        if (current.x <= 0 || current.y <= 0 || current == activeCoords[index] || !pathfinding.grid[current.x, current.y].walkable)
                        {
                            return false;
                        }

                        if (index != activeCoords.Count - 1)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    }
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnEnemy(EnemyType enemyList, Vector2 spawnpoint)
    {
        activeEnemies.Add(Instantiate(enemyParent, spawnpoint, Quaternion.identity, transform));
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