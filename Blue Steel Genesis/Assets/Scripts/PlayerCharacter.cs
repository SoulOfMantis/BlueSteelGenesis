using UnityEngine;
using BlueSteelGenesis.Character;

public class PlayerCharacter : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int currentEnergy;
    int maximumEnergy;
    bool myTurn;
    //Module[] modules = new Module[6];

    private bool executeModule(int n)
    {
        Debug.Log($"Исполнение модуля номер {n}");
        //if (modules[n] is not Module m)
        //    return false;
        //if (currentEnergy < m.EnergyCost) return false;
        //currentEnergy -= m.EnergyCost;
        //m.Effect();
        return true;
    }

    void TurnStart()
    {
        //executeAllPassiveTriggered(TriggerType.PlayerTurnStart);
        Debug.Log("Игрок начал ход!");
        myTurn = true;
        currentEnergy = maximumEnergy;
        //...
    }
    void TurnEnd()
    {
        Debug.Log("Игрок закончил ход!");
        //executeAllPassiveTriggered(TriggerType.PlayerTurnEnd);
        myTurn = false;
        //...
    }

    public override void damage(int dmg)
    {
        base.damage(dmg);
        //executeAllPassiveTriggered(TriggerType.PlayerTakesDamage);
        Debug.Log($"Игрок получил {dmg} урона!");
    }

    public override void heal(int hp)
    {
        base.heal(hp);
        //executeAllPassiveTriggered(TriggerType.PlayerHeals);
        Debug.Log($"Игрок полечился на {hp}!");
    }

    override protected void die()
    {
        //executeAllPassiveTriggered(TriggerType.PlayerDies);
        Debug.Log("Игрок умер!");
    }

    //private void executeAllPassiveTriggered(TriggerType t)
    //{
    //    for (int i = 0; i < modules.Length; i++)
    //        if (modules[i] is PassiveModule p)
    //            if (p.Trigger == t) executeModule(i);
    //}

    void ExecuteActive(int n)
    {
        Debug.Log($"Исполнение активного моделя {n}");
        //    if (modules[n] is ActiveModule)
                executeModule(n);
    }
}
