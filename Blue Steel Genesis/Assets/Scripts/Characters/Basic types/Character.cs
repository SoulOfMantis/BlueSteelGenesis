using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Character : Entity
{
    [SerializeField] protected CharacterVisualHandler visualHandler;

    public override async Task loseHealth(uint hp, ActionContext ctx) {
        ctx = ctx?.WithActionData(hp);
        await base.loseHealth(hp, ctx);
        await processTrigger(TriggerType.OnHealthLost, ctx);
    }
    public override async Task damage(uint dmg, ActionContext ctx)
    {
        dmg = Math.Max(dmg, 1);
        ctx = ctx?.WithActionData(dmg);
        await Awaitable.WaitForSecondsAsync(.1f); //TODO: remove delay; derived classes must await animations
        if (currentShield > 0)
        {
            uint shield_dmg = Math.Min(currentShield, dmg);
            dmg -= shield_dmg;
            await shieldDamage(shield_dmg, ctx);
        }
        if (dmg > 0)
        {
            currentHealth -= dmg;
            if (visualHandler != null)
                await visualHandler.PlayHurtAnimation(dmg);
            await changeColorAndWait(Color.crimson, 0.2f*dmg);
            await processTrigger(TriggerType.OnHealthDamage, ctx?.WithActionData(dmg));
            if (currentHealth == 0)
                await die();
        }
        await processTrigger(TriggerType.OnDamage, ctx);
    }
    public virtual async Task shieldDamage(uint shield_dmg, ActionContext ctx)
    {
        ctx = ctx?.WithActionData(shield_dmg);
        await loseShield(shield_dmg);
        await processTrigger(TriggerType.OnDamageShielded, ctx);
        Debug.Log($"{shield_dmg} урона поглощено щитом");
        if (currentShield == 0)
            await processTrigger(TriggerType.OnShieldBroken, ctx);
    }
    public virtual async Task loseShield(uint value)
    {
        currentShield -= value;
        UpdateTooltipIfCurrent();
        await Awaitable.WaitForSecondsAsync(.1f); //TODO: remove delay; derived classes must await animations
    }
    public override async Task heal(uint hp, ActionContext ctx)
    {
        ctx = ctx?.WithActionData(hp);
        await base.heal(hp, ctx);
        if (visualHandler != null)
            await visualHandler.PlayHealingAnimation(hp);
        await processTrigger(TriggerType.OnHeal, ctx);
    }

    public virtual async Task giveShield(uint amount, ActionContext ctx)
    {
        currentShield += Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        if (visualHandler != null)
            await visualHandler.PlayGainShieldAnimation(amount);
        await processTrigger(TriggerType.OnShieldGiven, ctx?.WithActionData(amount));
        Debug.Log($"Выдан щит: {amount}; Всего: {currentShield}");
    }
    public virtual async Task drainEnergy(uint amount, ActionContext ctx = null)
    {
        currentEnergy -= Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        await changeColorAndWait(Color.blue, 0.1f*amount);
        await processTrigger(TriggerType.OnEnergyDrain, ctx?.WithActionData(amount));
    }
    public virtual async Task restoreEnergy(uint amount, ActionContext ctx = null)
    {
        currentEnergy += Math.Max(amount, 1);
        UpdateTooltipIfCurrent();
        await changeColorAndWait(Color.aquamarine, 0.1f*amount);
        await processTrigger(TriggerType.OnEnergyRestore, ctx?.WithActionData(amount));
    }
    public virtual async Task startBattle()
    {
        if (visualHandler != null)
            await visualHandler.PlayStartBattleAnimation();
        await processTrigger(TriggerType.OnBattleStart, null);
    }
    public virtual async Task endBattle()
    {
        status_modules_.Clear();
        if (visualHandler != null)
            await visualHandler.PlayEndBattleAnimation();
        await processTrigger(TriggerType.OnBattleEnd, null);
    }
    public virtual async Task startTurn()
    {
        Debug.Log($"turn started");
        myTurn = true;
        if (visualHandler != null)
            await visualHandler.PlayStartTurnAnimation();
        await loseShield(currentShield.Value);
        await restoreEnergy(maxEnergy);
        await processTrigger(TriggerType.OnTurnStart, null);
    }
    public virtual async Task endTurn()
    {
        if (visualHandler != null)
            await visualHandler.PlayEndTurnAnimation();
        await processTrigger(TriggerType.OnTurnEnd, null);
        myTurn = false;
        tracker.NextTurn();
    }

    public async Task move(Vector3Int target_pos, List<Vector3Int> allowed, ActionContext ctx = null)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        await processTrigger(TriggerType.OnMove, Position.LeftBottom, ctx?.WithActionData(path));
    }
    public async Task move(PositionCollection target_pos, List<Vector3Int> allowed, ActionContext ctx = null)
    {
        var path = Navigation.Dijkstra.getPath(Position, target_pos, p => allowed.Contains(p));
        if (path == null)
            return;

        foreach (var step in path)
            await moveStep(step);
        await processTrigger(TriggerType.OnMove, Position.LeftBottom, ctx?.WithActionData(path));
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
        if (visualHandler != null)
            await visualHandler.PlayWalkAnimation(dir);
    }

    public Task strike(int x, int y, int z, uint dmg, ActionContext ctx) => strike(new Vector3Int(x, y, z), dmg, ctx);
    public async Task strike(Vector3Int pos, uint dmg, ActionContext ctx)
    {
        Entity target = tracker.FindEntityAtPosition(pos);
        if (target == null)
            return;
        
        if (visualHandler != null)
            await visualHandler.PlayAttackAnimation(pos);
  
        ctx = ctx?.WithActionData(dmg);
        await target.damage(dmg, ctx);
        await processTrigger(TriggerType.OnStrike, pos, ctx);
        Debug.Log($"Strike at {pos} for {dmg} damage");
    }

    public async Task apply(Vector3Int pos, StatusModule status, ActionContext ctx)
    {
        Character target = tracker.FindCharacterAtPosition(pos);
        if (target == null)
            return;
        await Awaitable.WaitForSecondsAsync(.2f);

        ctx = ctx?.WithActionData(status);
        await target.addStatusModule(status, ctx);
        await processTrigger(TriggerType.OnApply, pos, ctx);
        Debug.Log($"Apply {status.GetType().Name} at {pos}");
    }
    public async Task apply(StatusModule status, ActionContext ctx) => await apply(Position.RightTop, status, ctx);

    public new virtual Task<bool> summon<T>(PositionCollection pos) where T : Entity =>
        Task.FromResult(Entity.summon<T>(pos));


    public void addModule(GameModule module)
    {
        modules_.Add(module);
        module.Initialize();
    }
    public bool removeModule(GameModule module)
    {
        return modules_.Remove(module);
    }
    public async Task addStatusModule(StatusModule status, ActionContext ctx)
    {
        var module = status_modules_.Find(m => m.GetType() == status.GetType());
        if (module != null)
            module.Refresh(status);
        else
        {
            status_modules_.Add(status);
            UpdateTooltipIfCurrent();
            status.Initialize();
            await processTrigger(
                status switch {
                    NegativeStatusModule => TriggerType.OnNegativeStatusApplied,
                    PositiveStatusModule => TriggerType.OnPositiveStatusApplied,
                }, ctx?.WithActionData(status));

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
    protected Task processTrigger(TriggerType trigger, ActionContext ctx) => processTrigger(trigger, Position.LeftBottom, ctx);
    protected Task processTrigger(TriggerType trigger, Vector3Int pos, ActionContext ctx)
    {
        RechargeModules(trigger);
        return triggerModules(trigger, pos, ctx);
    }
    protected Task triggerModules(TriggerType triggerType, ActionContext context = null) => triggerModules(triggerType, Position.LeftBottom, context);
    protected async Task triggerModules(TriggerType triggerType, Vector3Int pos, ActionContext context = null)
    {
        foreach (var pm in listModules<PassiveModule>().Where(pm => pm.triggerType == triggerType))
        {
            Debug.Log(pm.Name + " triggering");
            if (isCorrectPosition(pm, pos))
            {
                await usePassiveModule_internal(pm, pos, context);
                Debug.Log(pm.Name + " triggered");
            }
        }
        await processStatusModules(triggerType, context);
    }
    protected async Task processStatusModules(TriggerType triggerType, ActionContext context = null)
    {
        foreach (var st in status_modules_.Where(m => triggerType == m.triggerType))
            await useStatusModule_internal(st, context);
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
    protected virtual Task useActiveModule_internal(ActiveModule m, Vector3Int pos) => m.Use(this, pos, null);
    protected virtual Task usePassiveModule_internal(PassiveModule m, Vector3Int pos, ActionContext ctx) => m.Use(this, pos, ctx);
    protected virtual Task useStatusModule_internal(StatusModule m, ActionContext ctx) => m.Use(this, Position.LeftBottom, ctx);

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
