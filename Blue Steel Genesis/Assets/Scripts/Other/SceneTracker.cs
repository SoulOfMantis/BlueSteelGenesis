using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneTracker : MonoBehaviour
{
    private InitiativeTracker init;
    private List<Entity> entities = new();
    public IReadOnlyList<Entity> Entities => entities;
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

    public Entity FindEntityAtPosition(Vector3Int pos) =>
        entities.Find(e => e.Position.Contains(pos));
    public Character FindCharacterAtPosition(Vector3Int pos) =>
        FindEntityAtPosition(pos) as Character;
    public Obstacle FindObstacleAtPosition(Vector3Int pos) =>
        FindEntityAtPosition(pos) as Obstacle;

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
        return FindEntityAtPosition(pos) != null;
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
    public void AddCharacter(Character character)
    {
        entities.Add(character);
        init.AddCharacter(character);
    }
    public void RemoveCharacter(Character character)
    {
        entities.Remove(character);
        init.RemoveCharacter(character);
    }
    public void AddObstacle(Obstacle obstacle) =>
        entities.Add(obstacle);
    public void RemoveObstacle(Obstacle obstacle) =>
        entities.Remove(obstacle);

    //                                         
    public PlayerCharacter getPlayer() =>
        init.getPlayer();
    public bool IsPlayerAlive() =>
        !init.CheckDefeat();

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
        Entity.tracker = this;
    }
}
