using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System;

public class PlayerCharacter : Character
{
    public static List<ModuleButton> activeModuleButtons = new();
    public TMP_Text energyDisplay;
    public TMP_Text healthDisplay;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    PlayerCharacter()
    {
        Initiative = 10;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tracker != null)
        {
            tracker.AddCharacter(this);
            Debug.Log("Player added");
        }
        VictoryScreen.SetActive(false);
        DefeatScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Vector3Int> GetModulePositions(int n) => getModule(n).getCellsInRange(Position).Where(c => isCorrectPosition(getModule(n), c)).ToList();

    void updateHealth()
    {
        healthDisplay.text = $"{currentHealth}/{maxHealth}";
    }

    void updateEnergy()
    {
        energyDisplay.text = $"{currentEnergy}/{maxEnergy}";
    }

    void updateButtons()
    {
        activeModuleButtons.ForEach(mb => mb.buttonInteractableManaging());
    }
    public string getmoduleName(int index)
    {
        if (!doesModuleExist(index)) return null;
        return modules_[index].Name;
    }
    public string getmoduleDescription(int index)
    {
        if (!doesModuleExist(index)) return null;
        return modules_[index].Description;
    }
    protected override async Task useActiveModule_internal(ActiveModule m, Vector3Int pos)
    {
        //play using module animation
        await base.useActiveModule_internal(m, pos);
    }
    protected override async Task usePassiveModule_internal(PassiveModule m, Vector3Int pos)
    {
        //play using module animation
        await base.usePassiveModule_internal(m, pos);
    }
    protected override async Task useStatusModule_internal(StatusModule m)
    {
        //play using module animation
        await base.useStatusModule_internal(m);
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

    public override async Task startBattle()
    {
        await base.startBattle();
        updateHealth();
        updateEnergy();
        updateButtons();
        //play starting battle animation
    }

    public override async Task endBattle()
    {
        await base.endBattle();
        //play ending battle animation
    }

    public override async Task startTurn()
    {
        await base.startTurn();
        //play start turn animation
    }

    public override async Task endTurn()
    {
        await base.endTurn();
        updateButtons();
        //play turn end animation
    }
    public void onEndTurnButtonPressed() =>
        StartCoroutine(TaskCoro.Make(endTurn()));

    public override async Task damage(int dmg)
    {
        Debug.Log($"����� ������� {dmg} �����!");
        await base.damage(dmg);
        updateHealth();
        //play taking damage animation
    }

    public override async Task heal(int hp)
    {
        Debug.Log($"����� ����������� {hp} ��������!");
        await base.heal(hp);
        updateHealth();
        //play healing animation
    }

    public override async Task drainEnergy(int amount)
    {
        await base.drainEnergy(amount);
        updateButtons();
        updateEnergy();
        //play losing energy animation
    }
    public override async Task restoreEnergy(int amount)
    {
        await base.restoreEnergy(amount);
        updateButtons();
        updateEnergy();
        //play restoring energy animation
    }

    override protected async Task die()
    {
        Debug.Log("����� ����!");
        tracker.RemoveCharacter(this);
        Defeat();
        //TODO: player loss
        //play dying animation
    }

    public async Task Victory()
    {
        updateButtons();
        await endBattle();
        VictoryScreen.SetActive(true);
    }
    public void Defeat()
    {
        updateButtons();
        DefeatScreen.SetActive(true);
    }


    public void GiveMoney(int value)=> PlayerMoney += value;
    public void LoseMoney(int value)=>PlayerMoney -= value;

    public int PlayerMoney
    {
        get => GameState.Run.Player.PlayerMoney;
        protected set => GameState.Run.Player.PlayerMoney = value;
    }
    public override int currentHealth {
        get => GameState.Run.Expedition.Player.currentHealth;
        protected set => GameState.Run.Expedition.Player.currentHealth = value;
    }
    public override int maxHealth {
        get => GameState.Run.Expedition.Player.maxHealth;
        protected set => GameState.Run.Expedition.Player.maxHealth = value;
    }
    public override int maxEnergy {
        get => GameState.Run.Expedition.Player.maxEnergy;
        protected set => GameState.Run.Expedition.Player.maxEnergy = value;
    }
    protected override List<GameModule> modules_ {
        get => GameState.Run.Expedition.Player.modules;
        set => GameState.Run.Expedition.Player.modules = value;
    }
}


