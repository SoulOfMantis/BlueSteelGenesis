using UnityEngine;

    public class Enemy : Character
    {
    public Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) {}

    protected override void die()
        {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
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
