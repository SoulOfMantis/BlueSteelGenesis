using UnityEngine;

public class TestObject : MonoBehaviour
{
    public int MyInt;
    public void PrintInt()
    {
        Debug.Log(MyInt);
    }
    public TestObject(int n)
    {
        MyInt = n;
    }

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
