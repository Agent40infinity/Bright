using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    Queue<PathRequested> pathRequestedQueue = new Queue<PathRequested>();
    PathRequested currentRequest;

    static PathfindingManager manager;
    Pathfinding pathfinding;
    bool isProcessing;

    private void Awake()
    {
        manager = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void PathRequest(Vector3 startPoint, Vector3 endPoint, Action<Vector3[], bool> callback)
    {
        PathRequested newRequest = new PathRequested(startPoint, endPoint, callback);
        manager.pathRequestedQueue.Enqueue(newRequest);
        manager.TryNextProcess();
    }

    void TryNextProcess()
    {
        if (!isProcessing && pathRequestedQueue.Count > 0)
        {
            currentRequest = pathRequestedQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindingPath(currentRequest.startPoint, currentRequest.endPoint);
        }
    }

    public void FinishedProcessing(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        TryNextProcess();
    }

    struct PathRequested
    {
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Action<Vector3[], bool> callback;

        public PathRequested(Vector3 _startPoint, Vector3 _endPoint, Action<Vector3[], bool> _callback)
        {
            startPoint = _startPoint;
            endPoint = _endPoint;
            callback = _callback;
        }
    }
}
