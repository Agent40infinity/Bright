using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public PathfindingNode parent;


    public PathfindingNode(bool isWalkable, Vector3 thisWorldPos, int gridPosX, int gridPosY) 
    {
        walkable = isWalkable;
        worldPosition = thisWorldPos;
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
}
