using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeTracker : MonoBehaviour
{

    public class TestObject
    {
        int MyInt;
        public void PrintInt()
        {
            Debug.Log(MyInt);
        }
        public TestObject(int n)
        {
            MyInt = n;
        }
    }
    List<TestObject> test = new();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            test.Add(new TestObject(i));
        }

    }

    void Update()
    {
        for (int i = 1;i < 10; i++)
        {
            test[i].PrintInt();
        }

    }
}
