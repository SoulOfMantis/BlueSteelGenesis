using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultilineRenderer2D : Graphic
{
    public struct Line {
        public Vector2 from, to;
        public float width;
        public Color color;
    }

    public void addLine(Line line) {
        lines_.Add(line);
        SetVerticesDirty();
    }
    public void clear() {
        lines_.Clear();
        SetVerticesDirty();
    }

    [ExecuteAlways]
    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();
        foreach (var line in lines_)
            vh.AddUIVertexQuad(toPoints(line));
    }
    private UIVertex[] toPoints(Line line) {
        var quad = new UIVertex[4];
        var vtx = UIVertex.simpleVert;
        vtx.color = line.color;
        Array.Fill(quad, vtx);

        var offset = (line.to - line.from).normalized * line.width / 2;
        (offset.x, offset.y) = (-offset.y, offset.x);
        quad[0].position = line.from + offset;
        quad[1].position = line.from - offset;
        quad[2].position = line.to - offset;
        quad[3].position = line.to + offset;

        quad[0].uv0 = new(0, 1);
        quad[1].uv0 = new(0, 0);
        quad[2].uv0 = new(1, 0);
        quad[3].uv0 = new(1, 1);

        return quad;
    }

    private List<Line> lines_ = new();
}
