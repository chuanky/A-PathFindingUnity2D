using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;
    PathRequestManager requestManager;
    private void Awake() {
        gridManager = GetComponent<GridManager>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 start, Vector3 target) {
        StartCoroutine(FindPath(start, target));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node[] waypoints = new Node[0];
        bool pathFound = false;

        Node startNode = gridManager.getNodeFromWorld(startPos);
        Node targetNode = gridManager.getNodeFromWorld(targetPos);

        if (startNode.walkable && targetNode.walkable) {
            MinHeap<Node> openSet = new MinHeap<Node>(gridManager.Size);
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node curr = openSet.Poll();
                closeSet.Add(curr);

                if (curr == targetNode) {
                    GetPath(startNode, targetNode);
                    sw.Stop();
                    print("path found in: " + sw.ElapsedMilliseconds + "ms");
                    pathFound = true;
                    break;
                }
                
                foreach (Node neighbour in gridManager.getNeighbours(curr))
                {
                    if (!neighbour.walkable || closeSet.Contains(neighbour)) continue;

                    int moveCost = curr.gCost + GetDistance(curr, neighbour);
                    if (moveCost < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = moveCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = curr;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
        }
        
        yield return null;
        if (pathFound) {
            waypoints = GetPath(startNode, targetNode);
        }
        requestManager.ProcessingDone(waypoints, pathFound);
    }

    Node[] GetPath(Node start, Node target) {
        List<Node> path = new List<Node>();
        Node curr = target;
        
        while (curr != start) {
            path.Add(curr);
            curr = curr.parent;
        }
        // Vector3[] waypoints = SimplifyPath(path);
        // List<Vector3> waypoints = new List<Vector3>();
        // for (int i = 0; i < path.Count; i++) {
        //     waypoints.Add(path[i].worldPos);
        // }
        // waypoints.Reverse();
        path.Reverse();
        return path.ToArray();
    }

    Vector3[] SimplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirOld = Vector2.zero;

        for (int i = 0; i < path.Count - 1; i++) {
            Vector2 dirNew = new Vector2(path[i].col - path[i + 1].col, path[i].row - path[i + 1].row);
            if (dirNew != dirOld) {
                waypoints.Add(path[i].worldPos);
            }

            dirOld = dirNew;
        }

        return waypoints.ToArray();
    }


    private int GetDistance(Node n1, Node n2) {
        int distRow = Mathf.Abs(n1.row - n2.row);
        int distCol = Mathf.Abs(n1.col - n2.col);

        return 10 * (distRow + distCol);
    }
}
