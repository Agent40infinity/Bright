using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public LayerMask wallMask;
    public Vector2 gridSize;
    public float nodeSize;
    PathfindingNode[,] grid;

    private int gridSizeX, gridSizeY;

    void Awake()
    {
        gridSizeX = Mathf.RoundToInt(gridSize.x / (nodeSize * 2));
        gridSizeY = Mathf.RoundToInt(gridSize.y / (nodeSize * 2));
        DrawGrid();
    }

    void DrawGrid()
    {
        grid = new PathfindingNode[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * ( gridSize.x / 2) - Vector3.up * gridSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 thisWorldPos = bottomLeft + Vector3.right * (x * (nodeSize * 2) + nodeSize) + Vector3.up * (y * (nodeSize * 2) + nodeSize);
                bool isWalkable = !(Physics.CheckSphere(thisWorldPos, nodeSize, wallMask));
                grid[x, y] = new PathfindingNode(isWalkable, thisWorldPos, x , y);
            }
        }
    }

    public List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        List<PathfindingNode> neighbours = new List<PathfindingNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public PathfindingNode NodeFromLocation(Vector3 worldPosition, string type)
    {
        float locationX = (worldPosition.x + gridSize.x / 2) / gridSize.x;
        float locationY = (worldPosition.y + gridSize.y / 2) / gridSize.y;
        //locationX = Mathf.Clamp01(locationX);
        //locationY = Mathf.Clamp01(locationY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * locationX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * locationY);
        Debug.Log(type + " " + x+ " " +y);
        return grid[x, y];
    }

    public List<PathfindingNode> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (grid != null)
        {
            foreach (PathfindingNode pathNode in grid)
            {
                Gizmos.color = (pathNode.walkable) ? Color.green : Color.red;
                if (path != null)
                    if (path.Contains(pathNode))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(pathNode.worldPosition, Vector3.one * ((nodeSize * 2)-.05f));
            }
        }
    }
}
