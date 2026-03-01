using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public virtual async Task damage(int dmg, ActionContext prevAction = null)
    {
        dmg = Math.Max(dmg, 1);
        var action = new ActionContext(null, "takeDamage", prevAction, this);
        if (currentShield > 0)
        {
            int shield_dmg = Math.Min(currentShield, dmg);
            currentShield -= shield_dmg;
            dmg -= shield_dmg;
            await triggerModules(TriggerType.OnDamageShielded, action);
            Debug.Log($"{shield_dmg} урона поглощено щитом");
            if (currentShield == 0)
                await triggerModules(TriggerType.OnShieldBroken, action);
        }
        if (dmg > 0)
        {
            currentHealth -= dmg;
            await triggerModules(TriggerType.OnHealthDamage, action);
            if (currentHealth == 0)
              await die();
        }
        await triggerModules(TriggerType.OnDamage, action);
    }

    public virtual async Task heal(int hp, ActionContext prevAction = null)
    {
        currentHealth += Math.Max(hp, 1);
        ActionContext action = new ActionContext(null, "heal", prevAction, this);
        await triggerModules(TriggerType.OnHeal, action);
    }

    public virtual async Task giveShield(int amount, ActionContext prevAction = null)
    {
        currentShield += Math.Max(amount, 1);
        ActionContext action = new ActionContext(null, "shielding", prevAction, this);
        await triggerModules(TriggerType.OnShieldGiven, action);
        Debug.Log($"Выдан щит: {amount}; Всего: {currentShield}");
    }
    abstract protected Task die();

    public virtual async Task drainEnergy(int amount, ActionContext prevAction = null)
    {
        currentEnergy -= Math.Max(amount, 1);
        ActionContext action = new ActionContext(null, "drainEnergy", prevAction, this);
        await triggerModules(TriggerType.OnEnergyDrain, action);
    }
    public virtual async Task restoreEnergy(int amount, ActionContext prevAction = null)
    {
        currentEnergy += Math.Max(amount, 1);
        ActionContext action = new ActionContext(null, "restoreEnergy", prevAction, this);
        await triggerModules(TriggerType.OnEnergyRestore, action);
    }


    public virtual async Task startBattle()
    {
        Position = tracker.WorldToCell(transform.position);
        await triggerModules(TriggerType.OnBattleStart);
    }
    public virtual async Task endBattle()
    {
        status_modules_.Clear();
        await triggerModules(TriggerType.OnBattleEnd);
    }
    public virtual async Task startTurn()
    {
        Debug.Log($"turn started");
        myTurn = true;
        currentShield = 0;
        await restoreEnergy(maxEnergy);
        await triggerModules(TriggerType.OnTurnStart);
    }
    public virtual async Task endTurn()
    {
        await triggerModules(TriggerType.OnTurnEnd);
        myTurn = false;
        tracker.NextTurn();
    }

    public async Task move(Vector3Int target_pos, List<Vector3Int> allowed, ActionContext prevAction = null)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        ActionContext action = new ActionContext(this, "move", prevAction);
        await triggerModules(TriggerType.OnMove, Position, action);
    }
    protected virtual async Task moveStep(Vector3Int dir)
    {
        Vector3Int new_pos = Position + dir;

        Vector3Int[] valid_moves = { Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up };
        if (!valid_moves.Contains(dir) || tracker.OutOfBounds(new_pos) || tracker.IsOccupied(new_pos)) return;
        Position = new_pos;

        await Awaitable.WaitForSecondsAsync(.2f); //TODO: remove delay; derived classes must await animations
    }

    public async Task strike(Vector3Int pos, int dmg, ActionContext prevAction = null)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        ActionContext action = new ActionContext(this, "strike", prevAction, target);
        await target.damage(dmg, action);
        await triggerModules(TriggerType.OnStrike, pos, action);
        Debug.Log($"Strike at {pos} for {dmg} damage");
    }

    public async Task apply(Vector3Int pos, StatusModule status, ActionContext prevAction = null)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        ActionContext action = new ActionContext(this, "apply", prevAction, target);
        await target.addStatusModule(status, action);
        await triggerModules(TriggerType.OnApply, pos, action);
        Debug.Log($"Apply {status.GetType().Name} at {pos}");
    }


    public void addModule(GameModule module)
    {
        modules_.Add(module);
        module.Initialize();
    }
    public async Task addStatusModule(StatusModule status, ActionContext prevAction = null)
    {
        var module = status_modules_.Find(m => m.GetType() == status.GetType());
        if (module != null)
            module.Refresh(status);
        else
        {
            status_modules_.Add(status);
            status.Initialize();
            if (status is NegativeStatus)
            {
                var action = new ActionContext(null, "addNegativeStatusModule", prevAction, this);
                await triggerModules(TriggerType.OnNegativeStatusApplied, action);
             }
            else if (status is PositiveStatus)
            {
                var action = new ActionContext(null, "addPositiveStatusModule", prevAction, this);
                await triggerModules(TriggerType.OnPositiveStatusApplied, action);
            }
            Debug.Log($"Status module {status.GetType().Name} added to {GetType().Name}");
        }
    }
    public async Task<bool> useActiveModule(int moduleIndex, Vector3Int pos)
    {
        var activeModule = getModule<ActiveModule>(moduleIndex);
        if (hasEnoughEnergy(activeModule) && isCorrectPosition(activeModule, pos))
        {
            await drainEnergy(activeModule.energyCost);
            await useActiveModule_internal(activeModule, pos);
            return true;
        }
        return false;
    }
    protected Task triggerModules(TriggerType triggerType, ActionContext context = null) => triggerModules(triggerType, Position, context);
    protected async Task triggerModules(TriggerType triggerType, Vector3Int pos, ActionContext context = null)
    {
        foreach (var pm in listModules<PassiveModule>().Where(pm => pm.triggerType == triggerType))
        {
            Debug.Log(pm.Name + " triggering");
            if (isCorrectPosition(pm, pos))
            {
                pm.loadContext(context);
                await usePassiveModule_internal(pm, pos);
                Debug.Log(pm.Name + " triggered");
            }
        }
        await processStatusModules(triggerType, context);
    }
    protected async Task processStatusModules(TriggerType triggerType, ActionContext context = null)
    {
        foreach (var st in status_modules_.Where(m => triggerType == m.triggerType))
        {
            st.loadContext(context);
            await useStatusModule_internal(st);
        }
        status_modules_.RemoveAll(m => m.IsExpired());
    }


    protected IEnumerable<ModuleT> listModules<ModuleT>()
        where ModuleT : GameModule
    {
        return modules_.Where(m => m is ModuleT).Select(m => m as ModuleT);
    }
    protected ModuleT getModule<ModuleT>(int module_index)
        where ModuleT : GameModule
    {
        var module = modules_.ElementAtOrDefault(module_index);
        return module as ModuleT;
    }
    protected GameModule getModule(int module_index)
    {
        return getModule<GameModule>(module_index);
    }

    public bool isPassive(int module_index) => getModule<PassiveModule>(module_index) != null;
    public bool isActive(int module_index) => getModule<ActiveModule>(module_index) != null;
    public bool doesModuleExist(int module_index) => getModule<GameModule>(module_index) != null;
    protected virtual bool isCorrectPosition(GameModule module, Vector3Int pos) => module.checkPosition(this, pos);
    protected virtual bool hasEnoughEnergy(ActiveModule module) => module != null && currentEnergy >= module.energyCost;
    protected virtual Task useActiveModule_internal(ActiveModule m, Vector3Int pos) => m.Effect(this, pos);
    protected virtual Task usePassiveModule_internal(PassiveModule m, Vector3Int pos) => m.Effect(this, pos);
    protected virtual Task useStatusModule_internal(StatusModule m) => m.Effect(this, Position);


    public abstract int currentHealth { get; protected set; }
    public abstract int maxHealth { get; protected set; }
    public int currentShield { get; protected set; }


    public int currentEnergy
    {
        get => current_energy_;
        protected set => current_energy_ = Math.Clamp(value, 0, maxEnergy);
    }
    public abstract int maxEnergy { get; protected set; }
    public int Initiative { get; protected set; }

    public bool myTurn { get; protected set; }

    public Vector3Int Position
    {
        get => position_;
        protected set
        {
            transform.position = tracker.CellToWorld(value);
            position_ = value;
        }
    }


    public static SceneTracker tracker;

    protected abstract List<GameModule> modules_ { get; set; }
    protected List<StatusModule> status_modules_ = new();

    private int current_energy_;
    private Vector3Int position_;
}
