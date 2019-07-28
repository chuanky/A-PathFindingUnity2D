using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveManager : MonoBehaviour
{
    Queue<MoveRequest> moveRequestQueue = new Queue<MoveRequest>();
    MoveRequest currMoveRequest;
    bool isProcessing;
    static MoveManager instance;
    
    private void Awake() {
        instance = this;
    }
    public static void RequestMove(Node start, Node end, Action<bool> callback) {
        MoveRequest newRequest = new MoveRequest(start, end, callback);
        instance.moveRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessing && moveRequestQueue.Count > 0) {
            currMoveRequest = moveRequestQueue.Dequeue();
            isProcessing = true;
            if (!currMoveRequest.end.reserved) {
                currMoveRequest.end.reserved = true;
                currMoveRequest.callback(true);
            } else {
                currMoveRequest.callback(false);
            }
        }
    }

    public void ProcessingDone () {
        isProcessing = false;
        TryProcessNext();
    }

    struct MoveRequest {
        public Node start;
        public Node end;
        public Action<bool> callback;

        public MoveRequest(Node _start, Node _end, Action<bool> _callback) {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }
}
