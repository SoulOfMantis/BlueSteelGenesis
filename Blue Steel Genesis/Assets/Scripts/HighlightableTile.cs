using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// Reference for tile highlighting
public readonly struct HighlightableTile
{
    public readonly TileBase BaseTile;
    public readonly TileBase HighlightedTile;
}