using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currPathRequest;
    Pathfinder pathFinder;
    bool isProcessing;
    static PathRequestManager instance;
    
    private void Awake() {
        instance = this;
        pathFinder = GetComponent<Pathfinder>();
    }
    public static void RequestPath(Vector3 start, Vector3 end, Action<Node[], bool> callback) {
        PathRequest newRequest = new PathRequest(start, end, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessing && pathRequestQueue.Count > 0) {
            currPathRequest = pathRequestQueue.Dequeue();
            isProcessing = true;
            pathFinder.StartFindPath(currPathRequest.start, currPathRequest.end);
        }
    }

    public void ProcessingDone (Node[] path, bool success) {
        currPathRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }

    struct PathRequest {
        public Vector3 start;
        public Vector3 end;
        public Action<Node[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Node[], bool> _callback) {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }
}
