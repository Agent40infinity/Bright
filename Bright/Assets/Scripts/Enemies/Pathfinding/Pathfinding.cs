using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    PathfindingGrid grid;

    void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startingPos, Vector3 targetPos)
    {
        PathfindingNode startingPoint = grid.NodeFromLocation(startingPos, "startingPos");
        PathfindingNode targetPoint = grid.NodeFromLocation(targetPos, "targetPos");

        List<PathfindingNode> openList = new List<PathfindingNode>();
        HashSet<PathfindingNode> closedList = new HashSet<PathfindingNode>();
        openList.Add(startingPoint);

        while (openList.Count > 0)
        {
            Debug.Log(openList.Count);
            PathfindingNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if(openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost)
                {
                    if (openList[i].hCost < currentNode.hCost)
                        currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetPoint)
            {
                RetracePath(startingPoint, targetPoint);
                return;
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
                        openList.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(PathfindingNode startNode, PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
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
