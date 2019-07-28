using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Debugger : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform player;
    private HashSet<Vector3Int> validPositions;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        validPositions = new HashSet<Vector3Int>();
        BoundsInt.PositionEnumerator positions = tilemap.cellBounds.allPositionsWithin;
        // float count = 0;
        while (positions.MoveNext()) {
            if (tilemap.HasTile(positions.Current)) {
                // count++;
                // Debug.Log(positions.Current);
                validPositions.Add(new Vector3Int(positions.Current.x, positions.Current.y, 0));
            }
        }
        var validPosItr = validPositions.GetEnumerator();
        validPosItr.MoveNext();
        player.position = validPosItr.Current;
        validPosItr.MoveNext();
        targetPos = validPosItr.Current;
        Debug.Log(validPosItr.Current);

        // Debug.Log("total valid tiles: " + count);
        // Debug.Log("all position found!");
        // displaySet(validPositions);
    }

    // Update is called once per frame
    void Update()
    {
        player.position = Vector3.MoveTowards(player.position, targetPos, 1 * Time.deltaTime);
    }

    private void displaySet(HashSet<Vector3Int> set)
    {
        foreach (Vector3Int pos in set)
        {
            Debug.Log(pos);
        }
    }
}
