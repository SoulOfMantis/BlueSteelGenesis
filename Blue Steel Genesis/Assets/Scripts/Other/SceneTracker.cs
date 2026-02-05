using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneTracker : MonoBehaviour
{
    private InitiativeTracker init;
    private List<Obstacle> obstacles = new();
    public Tilemap tl;
    public int max_y { get; private set; } = 3;
    public int max_x { get; private set; } = 17;
    private float CameraDistance = 10;
    public List<HighlightableTile> tiles;

    public void HighlightCharacterInInitiative(Character c, Color color)
    {
        init.HighlightCharacterInInitiative(c, color);
    }
    public void HighlightCharacterInInitiative(Character c) => HighlightCharacterInInitiative(c, Color.yellow);
    public void UnhighlightCharacterInInitiative(Character c)
    {
        init.UnhighlightCharacterInInitiative(c);
    }
    public Character FindCharacterAtPosition(Vector3Int pos)
    {
        return init.characters.Find(c => c.Position == pos);
    }
    public Obstacle FindObstacleAtPosition(Vector3Int pos)
    {
        return obstacles.Find(o => o.Position == pos);
    }
    public bool IsOccupiedByCharacter(Vector3Int pos)
    {
        return (FindCharacterAtPosition(pos) != null);
    }

    public bool IsOccupiedByObstacle(Vector3Int pos)
    {
        return (FindObstacleAtPosition(pos) != null);
    }

    public bool IsOccupied(Vector3Int pos)
    {
        return IsOccupiedByCharacter(pos) || IsOccupiedByObstacle(pos);
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
    public IEnumerable<Vector3Int> GetNeighborTiles(Vector3Int pos)
    {
        return new List<Vector3Int>() { Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up }
            .Select(v => v + pos)
            .Where(p => !OutOfBounds(p));
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
            HighlightCell(cell);
    }
    public void HighlightCell(Vector3Int cell)
    {
        var m = tiles.FindIndex(t => tl.GetTile(cell) == t.BaseTile);
        if (m >= 0)
            tl.SetTile(cell, tiles[m].HighlightedTile);
    }

    // Clears the highlighted cells
    public void ClearHighlights(List<Vector3Int> cells)
    {
        foreach (var cell in cells)
            UnhighlightCell(cell);
    }
    public void UnhighlightCell(Vector3Int cell)
    {
        var n = tiles.FindIndex(t => tl.GetTile(cell) == t.HighlightedTile);
        if (n >= 0)
            tl.SetTile(cell, tiles[n].BaseTile);
    }
    public void NextTurn()
    {
        init.StartNextTurn();
    }


    void Start()
    {
        init = gameObject.AddComponent(typeof(InitiativeTracker)) as InitiativeTracker;
        Character.tracker = this;
    }

    void Update()
    {

    }
}
