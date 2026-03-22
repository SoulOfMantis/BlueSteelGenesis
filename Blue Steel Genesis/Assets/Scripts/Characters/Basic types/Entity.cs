using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    void Start() => Init();
    protected virtual void Init() {
        if (tracker == null)
            return;

        Position = new(
            tracker.WorldToCell(transform.position) - new Vector3Int((int)bodySize, (int)bodySize) / 2,
            (int)bodySize
        );

        if (this is Character ch)
            tracker.AddCharacter(ch);
        else if (this is Obstacle o)
            tracker.AddObstacle(o);
        else
            Debug.LogWarning("Enity is not added");

        EntityInfoTooltipSetup();
    }
    void EntityInfoTooltipSetup()
    {
        gameObject.AddComponent<EntityTooltipTrigger>().entity = this;
        gameObject.AddComponent<BoxCollider2D>();
    }

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

    public PositionCollection Position
    {
        get => position_;
        protected set
        {
            transform.position = (tracker.CellToWorld(value.LeftBottom) +
                                  tracker.CellToWorld(value.RightTop)) / 2;
            position_ = value;
        }
    }
    [SerializeField, Tooltip("Размер сущности на поле")]
    protected uint bodySize = 1;

    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public static SceneTracker tracker;

    private PositionCollection position_;



    public static bool summon<T>(PositionCollection pos) where T : Entity {
        if (pos.Any(p => tracker.OutOfBounds(p) || tracker.IsOccupied(p)))
            return false;

        string folder;
        if (typeof(T).IsSubclassOf(typeof(Ally)))
            folder = "Ally";
        else if (typeof(T).IsSubclassOf(typeof(Enemy)))
            folder = "Enemy";
        else
            return false;

        string prefab_path = $"Prefabs/Entity/{folder}/{typeof(T)}";
        GameObject entity_prefab = Resources.Load<GameObject>(prefab_path);
        if (entity_prefab == null)
            return false;

        GameObject entity_object = Instantiate(entity_prefab);
        if (entity_object.transform.GetComponentInChildren<T>() is Entity entity)
            entity.Position = pos;
        else
            Debug.LogWarning("Could not set position!");
        return entity_object != null;
    }
}
