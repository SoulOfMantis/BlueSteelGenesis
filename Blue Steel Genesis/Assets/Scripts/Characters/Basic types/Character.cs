using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Character : Entity
{
    public override async Task damage(uint dmg)
    {
        dmg = Math.Max(dmg, 1);
        await Awaitable.WaitForSecondsAsync(.1f); //TODO: remove delay; derived classes must await animations
        if (currentShield > 0)
        {
            uint shield_dmg = Math.Min(currentShield, dmg);
            dmg -= shield_dmg;
            await shieldDamage(shield_dmg);
        }
        if (dmg > 0)
        {
            currentHealth -= dmg;
            await changeColorAndWait(Color.crimson, 0.2f*dmg);
            await processTrigger(TriggerType.OnHealthDamage);
            if (currentHealth == 0)
                await die();
        }
    }
    public virtual async Task shieldDamage(uint shield_dmg)
    {
        await loseShield(shield_dmg);
        await processTrigger(TriggerType.OnDamageShielded);
        Debug.Log($"{shield_dmg} урона поглощено щитом");
        if (currentShield == 0)
            await processTrigger(TriggerType.OnShieldBroken);
    }
    public virtual async Task loseShield(uint value)
    {
        currentShield -= value;
        UpdateTooltipIfCurrent();
        await Awaitable.WaitForSecondsAsync(.1f); //TODO: remove delay; derived classes must await animations
    }
    public override async Task heal(uint hp)
    {
        await base.heal(hp);
        await processTrigger(TriggerType.OnHeal);
    }

    public virtual async Task giveShield(uint amount)
    {
        currentShield += Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        await Awaitable.WaitForSecondsAsync(.2f); //TODO: remove delay; derived classes must await animations
        await processTrigger(TriggerType.OnShieldGiven);
        Debug.Log($"Выдан щит: {amount}; Всего: {currentShield}");
    }
public virtual async Task drainEnergy(uint amount)
    {
        currentEnergy -= Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        await changeColorAndWait(Color.blue, 0.1f*amount);
        await processTrigger(TriggerType.OnEnergyDrain);
    }
    public virtual async Task restoreEnergy(uint amount)
    {
        currentEnergy += Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        await changeColorAndWait(Color.aquamarine, 0.1f*amount);
        await processTrigger(TriggerType.OnEnergyRestore);
    }
    public virtual async Task startBattle()
    {
        await Awaitable.WaitForSecondsAsync(.5f); //TODO: remove delay; derived classes must await animations
        await processTrigger(TriggerType.OnBattleStart);
    }
    public virtual async Task endBattle()
    {
        status_modules_.Clear();
        await Awaitable.WaitForSecondsAsync(.5f); //TODO: remove delay; derived classes must await animations
        await processTrigger(TriggerType.OnBattleEnd);
    }
    public virtual async Task startTurn()
    {
        Debug.Log($"turn started");
        myTurn = true;
        await Awaitable.WaitForSecondsAsync(.2f); //TODO: remove delay; derived classes must await animations
        await loseShield(currentShield.Value);
        await restoreEnergy(maxEnergy);
        await processTrigger(TriggerType.OnTurnStart);
    }
    public virtual async Task endTurn()
    {
        if (myTurn)
        {
            await Awaitable.WaitForSecondsAsync(.1f); //TODO: remove delay; derived classes must await animations                                                     
            await processTrigger(TriggerType.OnTurnEnd);            
            myTurn = false;
            tracker.NextTurn();
        }
    }

    public async Task move(Vector3Int target_pos, List<Vector3Int> allowed)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        await processTrigger(TriggerType.OnMove, Position.LeftBottom);
    }
    public async Task move(PositionCollection target_pos, List<Vector3Int> allowed)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        await processTrigger(TriggerType.OnMove, Position.LeftBottom);
    }
    protected virtual async Task moveStep(Vector3Int dir)
    {
        var new_pos = Position + dir;

        Vector3Int[] valid_moves = { Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up };
        if (!valid_moves.Contains(dir) ||
            new_pos.Except(Position).Any(p => tracker.OutOfBounds(p)) ||
            new_pos.Except(Position).Any(p => tracker.IsOccupiedByCharacter(p)))
            return;
        Position = new_pos;

        await Awaitable.WaitForSecondsAsync(.2f); //TODO: remove delay; derived classes must await animations
    }

    public Task strike(int x, int y, int z, uint dmg) => strike(new Vector3Int(x, y, z), dmg);
    public async Task strike(Vector3Int pos, uint dmg)
    {
        Entity target = tracker.FindEntityAtPosition(pos);
        if (target == null)
            return;
        await Awaitable.WaitForSecondsAsync(.3f);
        await target.damage(dmg);
        await processTrigger(TriggerType.OnStrike, pos);
        Debug.Log($"Strike at {pos} for {dmg} damage");
    }

    public Task apply(int x, int y, int z, StatusModule status) => apply(new Vector3Int(x, y, z), status);
    public async Task apply(Vector3Int pos, StatusModule status)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        await Awaitable.WaitForSecondsAsync(.2f);
        target.addStatusModule(status);
        await processTrigger(TriggerType.OnApply, pos);
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
            UpdateTooltipIfCurrent();
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
    protected Task processTrigger(TriggerType trigger) => processTrigger(trigger, Position.LeftBottom);
    protected Task processTrigger(TriggerType trigger, Vector3Int pos)
    {
        RechargeModules(trigger);
        return triggerModules(trigger, pos);
    }
    protected Task triggerModules(TriggerType triggerType) => triggerModules(triggerType, Position.LeftBottom);
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
    protected void RechargeModules(TriggerType trigger) => modules_.ForEach(m => m.Recharge(trigger));

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
    protected virtual async Task useActiveModule_internal(ActiveModule m, Vector3Int pos)
    { await m.Use(this, pos); }
    protected virtual async Task usePassiveModule_internal(PassiveModule m, Vector3Int pos)
    { await m.Use(this, pos); }
    protected virtual async Task useStatusModule_internal(StatusModule m)
    { await m.Use(this, Position.LeftBottom); }

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

    public URangeValue currentShield { get; protected set; } = new();
    public URangeValue currentEnergy { get; protected set; } = new();
    public abstract uint maxEnergy { get; protected set; }
    public int Initiative { get; protected set; }

    public bool myTurn { get; protected set; }

    protected abstract List<GameModule> modules_ { get; set; }
    public IReadOnlyList<GameModule> Modules { get => modules_.AsReadOnly();}
    protected List<StatusModule> status_modules_ = new();
    public IReadOnlyList<GameModule> Statuses { get => status_modules_.AsReadOnly(); }
}
