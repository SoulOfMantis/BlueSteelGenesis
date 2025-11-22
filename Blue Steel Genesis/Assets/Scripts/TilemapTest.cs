using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     Vector3 pos = transform.position;
        pos = tilemap.GetCellCenterWorld(tilemap.WorldToCell(pos));
        transform.position = pos;
    }

    List<Vector3Int> getAvailableCells(int n, Vector3Int start)
    {
        Assert.IsTrue(n >= 0);
        List<Vector3Int> res = new List<Vector3Int>();
        HashSet<Vector3Int> toAdd = new HashSet<Vector3Int>();
        res.Add(start);
        for (int i = 1; i <= n; i++)
        {
            foreach (var cell in res )
            {
                toAdd.Add(new Vector3Int(cell.x + 1, cell.y));
                toAdd.Add(new Vector3Int(cell.x -1 , cell.y ));
                toAdd.Add(new Vector3Int(cell.x, cell.y + 1));
                toAdd.Add(new Vector3Int(cell.x, cell.y - 1));

            }
            foreach (var cell in toAdd)
            {
                if (!res.Contains(cell))
                    res.Add(cell);
            }
            toAdd.Clear();
        }
        return res;
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
