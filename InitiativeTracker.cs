using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeTracker : MonoBehaviour
{

    public List<TestObject> test;

    void Start()
    {


    }

    void Update()
    {
        for (int i = 1;i < test.Count; i++)
        {
            test[i].move();
        }

    }
}
