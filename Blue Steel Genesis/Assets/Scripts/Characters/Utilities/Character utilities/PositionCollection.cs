using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionCollection : IEnumerable<Vector3Int>
{
    public PositionCollection(Vector3Int leading_position, int side_size) =>
        (leading_position_, side_size_) = (leading_position, side_size);

    public bool Contains(Vector3Int p) =>
        p.x >= LeftBottom.x && p.y >= LeftBottom.y && p.x <= RightTop.x && p.y <= RightTop.y;

    public IEnumerable<Vector3Int> NeighborPositions() {
        Vector3Int lb = LeftBottom, rt = RightTop;
        for (int y = lb.y; y <= rt.y; ++y)
            yield return new(lb.x - 1, y);
        for (int x = lb.x; x <= rt.x; ++x)
            yield return new(x, lb.y - 1);
        for (int y = lb.y; y <= rt.y; ++y)
            yield return new(rt.x + 1, y);
        for (int x = lb.x; x <= rt.x; ++x)
            yield return new(x, rt.y + 1);
    }

    public static PositionCollection operator +(PositionCollection left, Vector3Int offset) =>
        new(left.leading_position_ + offset, left.side_size_);

    public IEnumerator<Vector3Int> GetEnumerator() =>
        new PositionEnumerator(leading_position_, side_size_);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object obj) {
        if (obj is PositionCollection p)
            return leading_position_ == p.leading_position_ && side_size_ == p.side_size_;
        return false;
    }
    public override int GetHashCode() =>
        (leading_position_, side_size_).GetHashCode();

    public Vector3Int LeftBottom => leading_position_;
    public Vector3Int RightTop => leading_position_ + new Vector3Int(side_size_ - 1, side_size_ - 1);
    public int SideSize => side_size_;

    private Vector3Int leading_position_;
    private readonly int side_size_ = 1;



    public static IEnumerable<PositionCollection> ContainingPositions(Vector3Int pos, int side_size) =>
        new PositionCollection(pos - new Vector3Int(side_size - 1, side_size - 1), side_size)
            .Select(lp => new PositionCollection(lp, side_size));

    public class PositionEnumerator : IEnumerator<Vector3Int> {
        public PositionEnumerator(Vector3Int leading_position, int side_size) =>
            (this.leading_position, this.side_size) = (leading_position, side_size);

        public bool MoveNext() =>
            ++idx_ < side_size * side_size;

        public void Reset() =>
            idx_ = -1;

        public void Dispose() {}

        public Vector3Int Current => leading_position + new Vector3Int(idx_ % side_size, idx_ / side_size);
        object IEnumerator.Current => Current;

        private readonly Vector3Int leading_position;
        private readonly int side_size;
        private int idx_ = -1;
    }
}
