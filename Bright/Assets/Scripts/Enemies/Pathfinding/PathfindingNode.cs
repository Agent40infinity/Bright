using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : IHeapItem<PathfindingNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public PathfindingNode parent;
    int heapIndex;


    public PathfindingNode(bool isWalkable, Vector3 thisWorldPos, int gridPosX, int gridPosY) 
    {
        walkable = isWalkable;
        worldPosition = thisWorldPos;
        //Debug.Log("Node Position: " + new Vector2(gridPosX, gridPosY) + "World Position: " + thisWorldPos);
        gridX = gridPosX;
        gridY = gridPosY;
    }

    public int fCost // will always be g+h
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(PathfindingNode nodeComparison)
    {
        int comparison = fCost.CompareTo(nodeComparison.fCost);
        if (comparison == 0)
        {
            comparison = hCost.CompareTo(nodeComparison.hCost);
        }
        return -comparison;
    }

}
