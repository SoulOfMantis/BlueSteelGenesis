using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using BlueSteelGenesis.Character_Modules;


public class SceneTracker : MonoBehaviour
{
    Character FindCharacterAtPosition(Vector3Int pos)
    {
        if (Character.position_ == pos) return null;
        return null;
    }
    Obstacle FindObstacleAtPosition(Vector3Int pos)
    {
        if (Obstacle.position_ == pos) return null;
        return null;
    }
    bool IsOccupied(Vector3Int pos)
    {
        if ((FindCharacterAtPosition(pos) != null) | (FindCharacterAtPosition(pos) != null)) return true;
        return false;
    }
    Vector3 CellToWorld(Vector3Int pos)
    {
        var tl = new Tilemap();
        return tl.CellToWorld(pos);
    }
    Vector3 WorldToCell(Vector3Int pos)
    {
        var tl = new Tilemap();
        return tl.WorldToCell(pos);
    }
    private Vector3Int GetCellByScreenPosition(Vector3 pos)
    {
        // TODO
        return null;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
