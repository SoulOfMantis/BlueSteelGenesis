using NUnit.Framework.Constraints;
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
        public Type type;
        
        public enum Type {
            NORMAL,
            DASHED,
            DISABLED
        }
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
            foreach (var subline in toPrimitiveLines(line))
                vh.AddUIVertexQuad(toPoints(subline));
    }

    private IEnumerable<Line> toPrimitiveLines(Line line) {
        switch (line.type) {
            case Line.Type.NORMAL:
                yield return line;
                yield break;
            case Line.Type.DISABLED:
                yield break;
        }

        const float gap = 8, target_len = 50;
        float len = (line.to - line.from).magnitude;

        int segment_count = (int)Math.Round(len / target_len);
        float segment_len = (len + gap) / segment_count;
        Vector2 segment = (line.to - line.from).normalized * segment_len;
        Vector2 subline = (line.to - line.from).normalized * (segment_len - gap);

        for (int i = 0; i < segment_count; ++i)
            yield return new() {
                from = line.from + segment * i,
                to = line.from + segment * i + subline,
                width = line.width,
                color = line.color,
                type = Line.Type.NORMAL
            };
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
