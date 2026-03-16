using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PlayerCharacter : Character
{
    public static List<ModuleButton> activeModuleButtons = new();
    public TMP_Text energyDisplay;
    public TMP_Text healthDisplay;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;

    [SerializeField] private ModuleButton[] moduleButtons;

    PlayerCharacter()
    {
        Name = "You";
        Description = "It's you! Robot, sent by humans to find and retrieve materials to repair their spaceship.";
        Initiative = 10;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnergy.Max = GameState.Run.Expedition.Player.maxEnergy;
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

    public override async Task damage(uint dmg)
    {
        Debug.Log($"Èãðîê ïîëó÷èë {dmg} óðîíà!");
        await base.damage(dmg);
        updateHealth();
        //play taking damage animation
    }

    public override async Task heal(uint hp)
    {
        Debug.Log($"Èãðîê âîññòàíîâèë {hp} çäîðîâüÿ!");
        await base.heal(hp);
        updateHealth();
        //play healing animation
    }

    public override async Task drainEnergy(uint amount)
    {
        await base.drainEnergy(amount);
        updateButtons();
        updateEnergy();
        //play losing energy animation
    }
    public override async Task restoreEnergy(uint amount)
    {
        await base.restoreEnergy(amount);
        updateButtons();
        updateEnergy();
        //play restoring energy animation
    }

    override protected async Task die()
    {
        Debug.Log("Èãðîê óìåð!");
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

    // Добавление нового модуля
    public void AddModule(GameModule module)
    {
        // Предотвращает дупликаты модулей
        if (modules_.Any(m => m.GetType() == module.GetType())) return;

        modules_.Add(module);

    }

    // Поменять два модуля местами
    public void SwapModules(int mod1, int mod2)
    {
        if (mod1 < 0 || mod2 < 0 || mod1 > modules_.Count || mod2 > modules_.Count) return;

        (modules_[mod1], modules_[mod2]) = (modules_[mod2], modules_[mod1]);

        RefreshModuleUI();
    }

    // Обновляет видимость кнопок
    public void RefreshModuleUI()
    {
        for (int i = 0;  i < moduleButtons.Length; i++)
        {
            var btn = moduleButtons[i];
            if (i < modules_.Count && modules_[i] is ActiveModule)
            {
                btn.gameObject.SetActive(true);
                btn.connectedModuleIndex = i;
                btn.RefreshDisplay();
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
        updateButtons();
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


