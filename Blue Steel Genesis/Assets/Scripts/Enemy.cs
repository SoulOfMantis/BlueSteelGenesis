using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using BlueSteelGenesis.Character_Modules;

    public class Enemy : Character
    {
    Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) {}

    protected override void die()
        {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
    }
    public override void startTurn()
    {
        base.startTurn();
        endTurn();
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
