using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public virtual async Task damage(int dmg)
    {
        dmg = Math.Max(dmg, 1);
        if (currentShield > 0)
        {
            int shield_dmg = Math.Min(currentShield, dmg);
            currentShield -= shield_dmg;
            dmg -= shield_dmg;
            await triggerModules(TriggerType.OnDamageShielded);
            Debug.Log($"{shield_dmg} урона поглощено щитом");
            if (currentShield == 0)
                await triggerModules(TriggerType.OnShieldBroken);
        }
        if (dmg > 0)
        {
            currentHealth -= dmg;
            await triggerModules(TriggerType.OnHealthDamage);
            if (currentHealth == 0)
                await die();
        }
    }
    public virtual async Task heal(int hp)
    {
        currentHealth += Math.Max(hp, 1);
        await triggerModules(TriggerType.OnHeal);
    }

    public virtual async Task giveShield(int amount)
    {
        currentShield += Math.Max(amount, 1);
        await triggerModules(TriggerType.OnShieldGiven);
        Debug.Log($"Выдан щит: {amount}; Всего: {currentShield}");
    }
    abstract protected Task die();
    public virtual async Task drainEnergy(int amount)
    {
        currentEnergy -= Math.Max(amount, 1);
        await triggerModules(TriggerType.OnEnergyDrain);
    }
    public virtual async Task restoreEnergy(int amount)
    {
        currentEnergy += Math.Max(amount, 1);
        await triggerModules(TriggerType.OnEnergyRestore);
    }

    private void CharacterInfoTooltipSetup()
    {
        gameObject.AddComponent<CharacterTooltipTrigger>().character = this;
        gameObject.AddComponent<BoxCollider2D>();
    }
    public virtual async Task startBattle()
    {
        CharacterInfoTooltipSetup();
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

    public async Task move(Vector3Int target_pos, List<Vector3Int> allowed)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        await triggerModules(TriggerType.OnMove, Position);
    }
    protected virtual async Task moveStep(Vector3Int dir)
    {
        Vector3Int new_pos = Position + dir;

        Vector3Int[] valid_moves = { Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up };
        if (!valid_moves.Contains(dir) || tracker.OutOfBounds(new_pos) || tracker.IsOccupied(new_pos)) return;
        Position = new_pos;

        await Awaitable.WaitForSecondsAsync(.2f); //TODO: remove delay; derived classes must await animations
    }

    public Task strike(int x, int y, int z, int dmg) => strike(new Vector3Int(x, y, z), dmg);
    public async Task strike(Vector3Int pos, int dmg)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        await target.damage(dmg);
        await triggerModules(TriggerType.OnStrike, pos);
        Debug.Log($"Strike at {pos} for {dmg} damage");
    }

    public Task apply(int x, int y, int z, StatusModule status) => apply(new Vector3Int(x, y, z), status);
    public async Task apply(Vector3Int pos, StatusModule status)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        target.addStatusModule(status);
        await triggerModules(TriggerType.OnApply, pos);
        Debug.Log($"Apply {status.GetType().Name} at {pos}");
    }


    public void addModule(GameModule module)
    {
        modules_.Add(module);
        module.Initialize();
    }
    public bool removeModule(GameModule module)
    {
        return modules_.Remove(module);
    }
    public void addStatusModule(StatusModule status)
    {
        var module = status_modules_.Find(m => m.GetType() == status.GetType());
        if (module != null)
            module.Refresh(status);
        else
        {
            status_modules_.Add(status);
            status.Initialize();
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
    protected Task triggerModules(TriggerType triggerType) => triggerModules(triggerType, Position);
    protected async Task triggerModules(TriggerType triggerType, Vector3Int pos)
    {
        foreach (var pm in listModules<PassiveModule>().Where(pm => pm.triggerType == triggerType))
        {
            if (isCorrectPosition(pm, pos))
                await usePassiveModule_internal(pm, pos);
        }
        await processStatusModules(triggerType);
    }
    protected async Task processStatusModules(TriggerType triggerType)
    {
        foreach (var st in status_modules_.Where(m => triggerType == m.triggerType))
            await useStatusModule_internal(st);
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

    public string getModuleName(int index)
    {
        if (!doesModuleExist(index)) return null;
        return getModule(index).Name;
    }
    public string getModuleDescription(int index)
    {
        if (!doesModuleExist(index)) return null;
        return getModule(index).Description();
    }
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

    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public static SceneTracker tracker;

    protected abstract List<GameModule> modules_ { get; set; }
    public IReadOnlyList<GameModule> Modules { get => modules_.AsReadOnly();}
    protected List<StatusModule> status_modules_ = new();
    public IReadOnlyList<GameModule> Statuses { get => status_modules_.AsReadOnly(); }

    private int current_energy_;
    private Vector3Int position_;

    public void removeModule(GameModule module)
    {
        //TODO
    }
}
