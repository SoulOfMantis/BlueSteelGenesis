using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : Character
{
    public Enemy(int maxHealth, int maxEnergy, int initiative)
    {
        this.maxHealth = maxHealth;
        this.maxEnergy = maxEnergy;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        Initiative = initiative;
    }

    protected override Task die()
    {
        Debug.Log($"{name} умер");
        tracker.RemoveCharacter(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public override int currentHealth { get; protected set; }
    public override int maxHealth { get; protected set; }
    public override int maxEnergy { get; protected set; }
    protected override List<GameModule> modules_ { get; set; } = new();
}
