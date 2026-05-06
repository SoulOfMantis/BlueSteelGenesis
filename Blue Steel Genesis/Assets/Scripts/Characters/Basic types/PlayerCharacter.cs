using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerCharacter : Character
{
    public static List<ModuleButton> activeModuleButtons = new();
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text energyDisplay;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthDisplay;
    [SerializeField] Slider shieldSlider;
    [SerializeField] TMP_Text shieldDisplay;

    [SerializeField] private ModuleButton[] moduleButtons;

    PlayerCharacter()
    {
        Name = "You";
        Description = "It's you! Robot, sent by humans to find and retrieve materials to repair their spaceship.";
        Initiative = 10;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Init()
    {
        base.Init();
        currentEnergy.Max = GameState.Run.Expedition.Player.maxEnergy;
        energySlider.maxValue = maxEnergy;
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Vector3Int> GetModulePositions(int n) => getModule(n).getCellsInRange(Position).Where(c => isCorrectPosition(getModule(n), c)).ToList();

    void updateHealth()
    {
        healthSlider.value = currentHealth;
        healthDisplay.text = $"{currentHealth}/{maxHealth}";
    }

    void updateEnergy()
    {
        energySlider.value = currentEnergy;
        energyDisplay.text = $"{currentEnergy}/{maxEnergy}";
    }
    void updateShields()
    {
        if (currentShield == 0)
        {
            shieldSlider.gameObject.SetActive(false);
            shieldDisplay.gameObject.SetActive(false);
            return;
        }
        shieldSlider.gameObject.SetActive(true);
        shieldDisplay.gameObject.SetActive(true);
        shieldSlider.value = currentShield;
        shieldDisplay.text = currentShield.Value.ToString();
    }
    void updateButtons()
    {
        activeModuleButtons.ForEach(mb => mb.buttonInteractableManaging());
    }
    protected override async Task useActiveModule_internal(ActiveModule m, Vector3Int pos)
    {
        //play using module animation
        await base.useActiveModule_internal(m, pos);

        if (false && !canUseAnyModule()) //TODO: check options
            await endTurn();
    }
    protected override async Task usePassiveModule_internal(PassiveModule m, Vector3Int pos, ActionContext ctx)
    {
        //play using module animation
        await base.usePassiveModule_internal(m, pos, ctx);
    }
    protected override async Task useStatusModule_internal(StatusModule m, ActionContext ctx)
    {
        //play using module animation
        await base.useStatusModule_internal(m, ctx);
    }

    protected override bool isCorrectPosition(GameModule module, Vector3Int pos)
    {
        var flag = base.isCorrectPosition(module, pos);
        if (!flag)
        {
            //play animation of incorrect position for module
        }
        return flag;
    }

    protected override bool hasEnoughEnergy(ActiveModule module)
    {
        var flag = base.hasEnoughEnergy(module);
        if (!flag)
        {
            //play animation of not enough energy for module
        }
        return flag;
    }

    public bool canUseModule(int module_index)
    {
        return myTurn && hasEnoughEnergy(getModule<ActiveModule>(module_index));
    }

    public override async Task giveShield(uint amount, ActionContext ctx)
    {
        await base.giveShield(amount, ctx);
        updateShields();
    }
    public override async Task loseShield(uint value)
    {
        await base.loseShield(value);
        updateShields();
    }
    public override async Task startBattle()
    {
        await base.startBattle();
        updateHealth();
        updateEnergy();
        updateShields();
        updateButtons();
    }

    public override async Task endBattle()
    {
        await base.endBattle();
    }

    public override async Task startTurn()
    {
        await base.startTurn();

        if (false && !canUseAnyModule()) //TODO: check options
            await endTurn();
    }

    public override async Task endTurn()
    {
        await base.endTurn();
        updateButtons();
    }
    public void onEndTurnButtonPressed() =>
        StartCoroutine(TaskCoro.Make(endTurn()));

    public override async Task loseHealth(uint hp, ActionContext ctx) {
        await base.loseHealth(hp, ctx);
        updateHealth();
    }

    public override async Task damage(uint dmg, ActionContext ctx)
    {
        Debug.Log($"Игрок получил {dmg} урона!");
        await base.damage(dmg, ctx);
        updateHealth();
    }

    public override async Task heal(uint hp, ActionContext ctx)
    {
        Debug.Log($"Игрок восстановил {hp} здоровья!");
        await base.heal(hp, ctx);
        updateHealth();
    }

    public override async Task drainEnergy(uint amount, ActionContext ctx = null)
    {
        await base.drainEnergy(amount, ctx);
        updateButtons();
        updateEnergy();
    }
    public override async Task restoreEnergy(uint amount, ActionContext ctx = null)
    {
        await base.restoreEnergy(amount, ctx);
        updateButtons();
        updateEnergy();
    }

    override protected async Task die()
    {
        Debug.Log("Игрок умер!");
        await processTrigger(TriggerType.OnDeath, null);
        if (TooltipSystem.IsCurrent(this))
        {
            TooltipSystem.Unlock(TooltipSystem.TooltipType.entityTooltip);
            TooltipSystem.Hide(TooltipSystem.TooltipType.entityTooltip);
        }
        await Awaitable.WaitForSecondsAsync(.5f);
        tracker.RemoveCharacter(this);
        Defeat();
        //TODO: player loss
        if (visualHandler != null)
            await visualHandler.PlayDeathAnimation();
    }

    public async Task Victory()
    {
        updateButtons();
        await endBattle();
        GameState.Run.Expedition.CombatSystem.Victory();
    }
    public void Defeat()
    {
        updateButtons();
        GameState.Run.Expedition.CombatSystem.Defeat();
    }
    public override URangeValue currentHealth {
        get => GameState.Run.Expedition.Player.currentHealth;
        protected set => GameState.Run.Expedition.Player.currentHealth = value;
    }
    public override uint maxHealth {
        get => GameState.Run.Expedition.Player.maxHealth;
        protected set => GameState.Run.Expedition.Player.maxHealth = value;
    }
    public override uint maxEnergy {
        get => GameState.Run.Expedition.Player.maxEnergy;
        protected set {
            currentEnergy.Max = value;
            GameState.Run.Expedition.Player.maxEnergy = value;
        }
    }
    protected override List<GameModule> modules_ {
        get => GameState.Run.Expedition.Player.modules;
        set => GameState.Run.Expedition.Player.modules = value;
    }
}


