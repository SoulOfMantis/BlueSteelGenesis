using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SceneTracker : MonoBehaviour
{
    private InitiativeTracker initiative;
    private List<Obstacle> obstacles = new();
    public Tilemap tl;
    private int max_y = 3;
    private int max_x = 17;
    private float CameraDistance = 10;
    public List<HighlightableTile> tiles;
    private List<Vector3Int> map = new List<Vector3Int>();

    public Character FindCharacterAtPosition(Vector3Int pos)
    {
        return initiative.characters.Find(c  => c.Position == pos);
    }
    public bool isAlive(Character c)
    { return initiative.isAlive(c); }
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
        return tl.GetCellCenterWorld(pos);
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
        initiative.AddCharacter(charact);
    }
    public void RemoveCharacter(Character character)
    {
        initiative.RemoveCharacter(character);
    }

    //                                         
    public PlayerCharacter getPlayer()
    {
        return initiative.characters.Find(c => c is PlayerCharacter) as PlayerCharacter;
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

    public void NextTurn()
    {
        initiative.StartNextTurn();
    }


    void Start()
    {
        initiative = gameObject.AddComponent(typeof(InitiativeTracker)) as InitiativeTracker;
        Character.tracker = this;
        map.Add(new Vector3Int(0, 0, 0));
        map.Add(new Vector3Int(0, 1, 0));
        //tiles.Add(new HighlightableTile(tl.GetTile(map[0]), tl.GetTile(map[1])));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HighlightCells(map);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            ClearHighlights(map);
        }

    }
}
