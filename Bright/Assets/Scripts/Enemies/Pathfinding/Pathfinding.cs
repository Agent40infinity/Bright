using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    PathfindingGrid grid;
    PathfindingManager manager;

    void Start()
    {
        grid = GetComponent<PathfindingGrid>();
        manager = GetComponent<PathfindingManager>();
    }

    public void StartFindingPath(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FindPath(startPoint, endPoint));
        // Debug.Log(startPoint + " " + endPoint);
    }

    IEnumerator FindPath(Vector3 startingPos, Vector3 targetPos)
    {
        PathfindingNode startingPoint = grid.NodeFromLocation(startingPos, "startingPos");
        PathfindingNode targetPoint = grid.NodeFromLocation(targetPos, "targetPos");

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        if (startingPoint.walkable && targetPoint.walkable)
        {
            PathfindingHeapCollection<PathfindingNode> openList = new PathfindingHeapCollection<PathfindingNode>(grid.MaxSize);
            HashSet<PathfindingNode> closedList = new HashSet<PathfindingNode>();
            openList.Add(startingPoint);

            while (openList.Count > 0)
            {
                PathfindingNode currentNode = openList.RemoveFirst();

                closedList.Add(currentNode);

                if (currentNode == targetPoint)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (PathfindingNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int costToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (costToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        neighbour.gCost = costToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetPoint);
                        neighbour.parent = currentNode;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                        else
                        {
                            openList.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startingPoint, targetPoint);
        }
        manager.FinishedProcessing(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(PathfindingNode startNode, PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = ReducePoints(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] ReducePoints(List<PathfindingNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i].worldPosition);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }

    int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
        
    }
}
