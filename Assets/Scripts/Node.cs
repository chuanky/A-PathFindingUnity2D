using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gCost;
    public int hCost;
    public int row;
    public int col;
    public Node parent;
    public bool reserved;
    private int heapIndex;

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public Node(bool walkable, Vector3 worldPos, int row, int col) {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.row = row;
        this.col = col;
        reserved = false;
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other) {
        int compare = fCost.CompareTo(other.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
