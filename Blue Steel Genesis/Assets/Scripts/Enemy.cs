using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using BlueSteelGenesis.Character_Modules;

    public class Enemy : Character
    {
    Enemy(int MaxHealth,int MaxEnergy) : base(MaxHealth, MaxEnergy) {} // TODO: move to derived classes

    protected override void die()
        {
        Debug.Log($"{name} умер");
        Tracker.RemoveCharacter(this);
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
