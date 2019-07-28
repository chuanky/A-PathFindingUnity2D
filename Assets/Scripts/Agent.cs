using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Agent : MonoBehaviour
{
    public bool debugMode;
    public Transform target;
    public float speed = 0.2f;
    Node[] path;
    int pathIndex;
    public MoveManager moveManager;

    private void Awake() {
        moveManager = GameObject.FindGameObjectWithTag("PathManager").GetComponent<MoveManager>();
    }
    private void Start() {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        if (moveManager == null) Debug.Log("attach moveManager to agent");
        path[0].reserved = true;
        transform.position = path[0].worldPos;
    }

    private void removeFirst() {
        path[0].reserved = false;
        path[1].reserved = true;
        if (transform.position == path[path.Length - 1].worldPos) path[1].reserved = false;
        var temp = new List<Node>(path);
        temp.RemoveAt(0);
        path = temp.ToArray();
    }

    public void OnPathFound(Node[] _path, bool pathFound) {
        if (pathFound) {
            path = _path;
            StopCoroutine("MoveOneStep");
            StartCoroutine(MoveOneStep(0));
            // StopCoroutine("FollowPath");
            // StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath() {
        Vector3 curr = path[0].worldPos;
        
        while (true) {
            if (transform.position == curr) {
                pathIndex++;
                if (pathIndex >= path.Length) {
                    yield break;
                }
                curr = path[pathIndex].worldPos;
            }

            transform.position = Vector3.MoveTowards(transform.position, curr, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveOneStep(int wait) {
        yield return new WaitForSeconds(wait);

        if (path.Length > 1) {
            MoveManager.RequestMove(path[0], path[1], onMoveRequestDone);
        }

        yield return null;
    }

    public void onMoveRequestDone(bool canMove) {
        if (canMove) {
            StopCoroutine("MoveToNextNode");
            StartCoroutine(MoveToNextNode());
        } else {
            StopCoroutine("MoveOneStep");
            StartCoroutine(MoveOneStep(1));
        }
        moveManager.ProcessingDone();
    }

    IEnumerator MoveToNextNode() {
        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, path[1].worldPos, speed * Time.deltaTime);
            if (transform.position == path[1].worldPos) {
                removeFirst();
                StopCoroutine("MoveOneStep");
                StartCoroutine(MoveOneStep(0));
                yield break;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos() {
        if (!debugMode) return;

        if (path != null) {
            for (int i = pathIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPos, new Vector3(0.5f, 0.5f, 0.5f));

                if (i == pathIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPos);
                } else {
                    Gizmos.DrawLine(path[i - 1].worldPos, path[i].worldPos);
                }
            }
        }
    }
}
