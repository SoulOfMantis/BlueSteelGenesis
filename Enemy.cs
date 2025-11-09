using UnityEngine;

    public class Enemy : Character
    {
    protected override void die()
        {
        Debug.Log($"{name} умер");
        Destroy(gameObject);
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
