using System;
using NUnit.Framework;

public class ArrayUtil_test
{
    [Test]
    public void join()
    {
        byte[] arr1 = {0, 1, 2}, arr2 = {99, 99, 255, 98}, arr3 = {17}, arr4 = {};
        Assert.AreEqual(ArrayUtil.join(arr1, arr1, arr4, arr3), new byte[]{0, 1, 2, 0, 1, 2, 17});
        Assert.AreEqual(ArrayUtil.join(arr4), new byte[]{});
        Assert.AreEqual(ArrayUtil.join(arr4, arr4), new byte[]{});
        Assert.AreEqual(ArrayUtil.join(arr2, arr3, arr1), new byte[]{99, 99, 255, 98, 17, 0, 1, 2});
        Assert.AreEqual(ArrayUtil.join(arr2, arr2), new byte[]{99, 99, 255, 98, 99, 99, 255, 98});
        Assert.AreEqual(ArrayUtil.join(arr3), new byte[]{17});
    }

    [Test]
    public void fromHexString() {
        Assert.AreEqual(ArrayUtil.fromHexString("01020304"), new byte[]{1, 2, 3, 4});
        Assert.AreEqual(ArrayUtil.fromHexString("90FA71"), new byte[]{0x90, 0xFA, 0x71});
        Assert.AreEqual(ArrayUtil.fromHexString("BC"), new byte[]{0xBC});
        Assert.AreEqual(ArrayUtil.fromHexString("bc"), new byte[]{0xBC});
        Assert.AreEqual(ArrayUtil.fromHexString(""), new byte[]{});

        Assert.Throws<ArgumentException>(() => ArrayUtil.fromHexString("126"));
        Assert.Throws<ArgumentException>(() => ArrayUtil.fromHexString("AA-BC"));
        Assert.Throws<FormatException>(() => ArrayUtil.fromHexString("7809io"));
        Assert.Throws<FormatException>(() => ArrayUtil.fromHexString("_098"));
    }

    [Test]
    public void toHexString() {
        byte[] arr1 = {0, 1, 2}, arr2 = {99, 99, 255, 98}, arr3 = {17}, arr4 = {};
        Assert.AreEqual(ArrayUtil.toHexString(arr1), "000102");
        Assert.AreEqual(ArrayUtil.toHexString(arr2), "6363FF62");
        Assert.AreEqual(ArrayUtil.toHexString(arr3), "11");
        Assert.AreEqual(ArrayUtil.toHexString(arr4), "");
    }
}
