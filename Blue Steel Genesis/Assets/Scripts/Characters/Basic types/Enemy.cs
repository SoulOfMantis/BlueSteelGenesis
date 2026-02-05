using System.Threading.Tasks;
using UnityEngine;

public class Enemy : Character
{
    public Enemy(int maxHealth, int maxEnergy, int initiative) : base(maxHealth, maxEnergy, initiative) 
    {
        Name = "Default enemy name";
        Description = "Default enemy description. If you see this, something went wrong.";
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
}
