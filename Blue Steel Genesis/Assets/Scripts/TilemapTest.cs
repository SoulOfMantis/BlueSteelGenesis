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

    // Update is called once per frame
    void Update()
    {
        
    }
}
