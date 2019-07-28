using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;
    public bool debugMode;
    Node[,] gridNodes;
    private Vector3 mapCenter;
    private HashSet<Vector3Int> validPositions;
    private BoundsInt cellBounds;
    private Grid grid;
    

    // Start is called before the first frame update
    void Awake()
    {
        cellBounds = tilemap.cellBounds;   
        grid = tilemap.layoutGrid;
        createGrid();
    }

    public int Size {
        get {
            return cellBounds.size.x * cellBounds.size.y;
        }
    }

    
    void createGrid() {
        int gridWidth = cellBounds.size.x;
        int gridHeight = cellBounds.size.y;
        float cellWidth = grid.cellSize.x;
        float cellHeight = grid.cellSize.y;
        // if (cellWidth % 2 != 0 || cellHeight % 2 != 0) {
        //    Debug.Log("cell width and height should both be even number so that we have center position without decimal");
        // }
        gridNodes = new Node[gridHeight, gridWidth];
        Vector3Int tilemapOrigin = tilemap.origin;

        for (int i = 0; i < gridHeight; i++) {
            for (int j = 0; j < gridWidth; j++) {
                Vector3Int currNodePos = new Vector3Int(tilemapOrigin.x + j, tilemapOrigin.y + i, 0);
                //cell origin is bottomLeft, if we have a sprite with center anchor,
                //we need to shift the worldPos to the cell center
                Vector3 currWorldPos = new Vector3(currNodePos.x + cellWidth / 2, currNodePos.y + cellHeight / 2, 0);
                gridNodes[i, j] = new Node(tilemap.HasTile(currNodePos), currWorldPos, i, j);
            }
        }
    }

    public Node getNodeFromWorld(Vector3 worldPos) {
        Vector3 origin = gridNodes[0, 0].worldPos;
        int row = Mathf.RoundToInt(worldPos.y - origin.y);
        int col = Mathf.RoundToInt(worldPos.x - origin.x);
        if (!inGrid(row, col)) return null;
        return gridNodes[row, col];
    }

    public List<Node> getNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        if (node == null) return neighbours;
        //up
        if (inGrid(node.row + 1, node.col)) neighbours.Add(gridNodes[node.row + 1, node.col]);
        //down
        if (inGrid(node.row - 1, node.col)) neighbours.Add(gridNodes[node.row - 1, node.col]);
        //left
        if (inGrid(node.row, node.col - 1)) neighbours.Add(gridNodes[node.row, node.col - 1]);
        //right
        if (inGrid(node.row, node.col + 1)) neighbours.Add(gridNodes[node.row, node.col + 1]);

        return neighbours;
    }

    private bool inGrid(int row, int col) {
        if (row < 0 || col < 0 || row >= gridNodes.GetLength(0) || col >= gridNodes.GetLength(1)) return false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawWireCube(transform.position, new Vector3(grid.cellSize.x, grid.cellSize.y, 1));
        if (gridNodes == null) return;

        if (debugMode) {
            Node playerNode = getNodeFromWorld(player.position);
            List<Node> neighbours = getNeighbours(playerNode);
            foreach (Node n in gridNodes)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode == n) Gizmos.color = Color.cyan;

                Gizmos.DrawCube(n.worldPos, new Vector3(grid.cellSize.x, grid.cellSize.y, 0));
            }
        }
        
    }
}
