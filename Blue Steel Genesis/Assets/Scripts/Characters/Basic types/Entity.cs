using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    void Start() {
        if (tracker == null)
            return;

        Position = tracker.WorldToCell(transform.position);

        if (this is Character ch)
            tracker.AddCharacter(ch);
        else if (this is Obstacle o)
            tracker.AddObstacle(o);
        else
            Debug.LogWarning("Enity is not added");

        Init();
    }
    protected virtual void Init() {}

    public virtual Task damage(uint dmg) {
        currentHealth -= Math.Max(dmg, 1);
        return currentHealth.Value switch {
            0 => die(),
            _ => Task.CompletedTask
        };
    }
    public virtual Task heal(uint hp) {
        currentHealth += Math.Max(hp, 1);
        return Task.CompletedTask;
    }
    abstract protected Task die();


    public abstract URangeValue currentHealth { get; protected set; }
    public abstract uint maxHealth { get; protected set; }

    public Vector3Int Position
    {
        get => position_;
        protected set
        {
            transform.position = tracker.CellToWorld(value);
            position_ = value;
        }
    }

    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public static SceneTracker tracker;

    private Vector3Int position_;
}
