using BlueSteelGenesis.Character_Modules;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SceneTracker : MonoBehaviour
{
    private InitiativeTracker init;
    private List<Obstacle> obstacles = new();
    public Tilemap tl;
    private int max_y = 4;
    private int max_x = 3;
    private float CameraDistance;
    public List<HighlightableTile> tiles;
    private List<Vector3Int> map = new List<Vector3Int>();

    public Character FindCharacterAtPosition(Vector3Int pos)
    {
        return init.characters.Find(c  => c.Position == pos);
    }
    public Obstacle FindObstacleAtPosition(Vector3Int pos)
    {
        return obstacles.Find(o => o.Position == pos);
    }
    public bool IsOccupied(Vector3Int pos)
    {
        return (FindCharacterAtPosition(pos) != null) || (FindObstacleAtPosition(pos) != null);
    }
    public Vector3 CellToWorld(Vector3Int pos)
    {
        return tl.CellToWorld(pos);
    }
    public Vector3Int WorldToCell(Vector3 pos)
    {
        return tl.WorldToCell(pos);
    }
    public Vector3Int GetCellByScreenPosition(Vector3 MousePosition)
    {
        MousePosition.z = CameraDistance;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(MousePosition);

        Vector3Int cell = tl.WorldToCell(worldPoint);

        if (OutOfBounds(cell))
        {
            return new Vector3Int(-1, -1, -1);
        }

        return cell;
    }
    public bool OutOfBounds(Vector3Int pos) 
    {
        return (pos.x > max_x) || (pos.y > max_y) || (pos.x < 0) || (pos.y < 0);
    }
    public void AddCharacter(Character charact)
    {
        init.AddCharacter(charact);
    }
    public void RemoveCharacter(Character character)
    {
        init.RemoveCharacter(character);
    }

    //                                         
    public PlayerCharacter getPlayer()
    {
        return init.characters.Find(c => c is PlayerCharacter) as PlayerCharacter;
    }

    // Highlights given cells
    public void HighlightCells(List<Vector3Int> cells)
    {
        foreach (var cell in cells)
        {
            var m = tiles.FindIndex(t => tl.GetTile(cell) == t.BaseTile);
            if (m >= 0)
            {
                tl.SetTile(cell, tiles[m].HighlightedTile);
            }
        }
    }

    // Clears the highlighted cells
    public void ClearHighlights(List<Vector3Int> cells)
    {
        foreach (var cell in cells)
        {
            var n = tiles.FindIndex(t => tl.GetTile(cell) == t.HighlightedTile);
            if (n >= 0)
            {
                tl.SetTile(cell, tiles[n].BaseTile);
            }
        }

    }


    void Start()
    {
        CameraDistance = tl.transform.position.z - Camera.main.transform.position.z;
        init = gameObject.AddComponent(typeof(InitiativeTracker)) as InitiativeTracker;
        Character.tracker = this;
        map.Add(new Vector3Int(0, 0, 0));
        map.Add(new Vector3Int(0, 1, 0));
        tiles.Add(new HighlightableTile(tl.GetTile(map[0]), tl.GetTile(map[1])));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            HighlightCells(map);
        }
        else if (Input.GetKey(KeyCode.U))
        {
            ClearHighlights(map);
        }

    }
}
