using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// Class used for highlighting cells
public class TileHighlighter : MonoBehaviour
{
    public Tilemap tm;
    public TileBase tile;
    public TileBase highlightedTile;

    // List of currently highlighted cells on tilemap
    private List<Vector3Int> currentlyHighlighted = new List<Vector3Int>();

    // Returns all highlighted cells to normal
    public void ClearHighlights()
    {
        foreach (var cell in currentlyHighlighted)
            tm.SetTile(cell, tile);
        currentlyHighlighted.Clear();
    }

    // Highlight given cells
    public void HighlightCells(List<Vector3Int> cells)
    {
        // Clear highlights in case of previous highlights still existing
        ClearHighlights();

        foreach (var cell in cells)
        {
            currentlyHighlighted.Add(cell);
            tm.SetTile(cell, highlightedTile);
        }
    }
}