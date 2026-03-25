using System;

public class URangeValue {
    public URangeValue(long val = 0, uint max = uint.MaxValue) {
        value_ = (uint)Math.Clamp(val, 0, max);
        max_ = max;
    }

    public uint Value {
        get => value_;
        set => value_ = Math.Min(value, max_);
    }
    public uint Max {
        get => max_;
        set {
            max_ = value;
            Value = value_;
        }
    }
    private uint value_, max_;

    public static URangeValue operator +(URangeValue rv, int val) => rv.Sum(val);
    public static URangeValue operator -(URangeValue rv, int val) => rv.Sum(-(long)val);
    public static URangeValue operator +(URangeValue rv, uint val) => rv.Sum(val);
    public static URangeValue operator -(URangeValue rv, uint val) => rv.Sum(-(long)val);
    public static URangeValue operator ++(URangeValue rv) => rv + 1;
    public static URangeValue operator --(URangeValue rv) => rv - 1;

    private URangeValue Sum(long val) {
        val = Math.Clamp(val, -value_, (long)max_ - value_);
        return new(val + value_, max_);
    }

    public static implicit operator uint(URangeValue v) =>
        v.value_;
    public static explicit operator int(URangeValue v) =>
        (int)v.value_;

    public override string ToString() =>
        value_.ToString();
}
