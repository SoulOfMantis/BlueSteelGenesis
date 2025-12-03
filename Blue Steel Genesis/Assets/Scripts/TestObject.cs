using UnityEngine;

public class TestObject : MonoBehaviour
{
    public int MyInt;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void move()
    {
        rb.MovePosition(rb.position + Vector2.right * MyInt);


       Debug.Log(MyInt);
    }
    public TestObject(int n)
    {
        MyInt = n;
    }

// Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        
    }
}
