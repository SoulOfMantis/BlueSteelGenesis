using BlueSteelGenesis.Character_Modules;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SceneTracker : MonoBehaviour
{
    private InitiativeTracker init;
    private List<Obstacle> obstacles;
    public Tilemap tl;
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
        return (FindCharacterAtPosition(pos) != null) | (FindCharacterAtPosition(pos) != null);
    }
    public Vector3 CellToWorld(Vector3Int pos)
    {
        return tl.CellToWorld(pos);
    }
    public Vector3 WorldToCell(Vector3Int pos)
    {
        return tl.WorldToCell(pos);
    }
    public Vector3Int GetCellByScreenPosition(Vector3 pos)
    {

        return Vector3Int.zero;
    }
    public bool OutOfBounds(Vector3Int pos) 
    {
        return false;
    }
    public void AddCharacter(Character charact)
    {
        init.characters.Add(charact);
    }
    public void RemoveCharacter(Character character)
    {
        init.characters.Remove(character);
    }


    void Start()
    {
        init = new InitiativeTracker();
        Character.tracker = this;
    }

    void Update()
    {
        
    }
}
