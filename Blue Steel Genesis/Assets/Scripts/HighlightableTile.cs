using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// Reference for tile highlighting
[System.Serializable]
public struct HighlightableTile
{
    public TileBase BaseTile;
    public TileBase HighlightedTile;

    public HighlightableTile(TileBase baseTile, TileBase highlightedTile)
    {
        BaseTile = baseTile;
        HighlightedTile = highlightedTile;
    }

}