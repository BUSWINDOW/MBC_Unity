using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SimlessTest : MonoBehaviour
{
    public Terrain terrain;
    public Transform player;

    private int cellCnt = 40;
    private int cellSizeX;
    private int cellSizeZ;

    Vector2Int currentCell;

    private void Start()
    {
        this.cellSizeX = (int)this.terrain.terrainData.size.x / cellCnt;
        this.cellSizeZ = (int)this.terrain.terrainData.size.z / cellCnt;
    }
    void Update()
    {
        Vector2Int newCell = GetCellFromPosition(player.transform.position);
        if (newCell != currentCell)
        {
            currentCell = newCell;
            Debug.Log($"Player is now in cell: {currentCell.x}, {currentCell.y}");
        }
    }

    Vector2Int GetCellFromPosition(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / cellSizeX);
        int y = Mathf.FloorToInt(pos.z / cellSizeZ);
        return new Vector2Int(x, y);
    }
}
