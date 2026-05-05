using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Rest
{
    [SerializeField]
    private uint biomeId;
    private const float freeHealModifier = 0.3f;
    private const float paidHealModifier = 1;
    public const uint PaidHealCost = 50;
    private bool healUsed;

    public Rest(uint biomeId)
    {
        this.biomeId = biomeId;
    }

    public void Trigger()
    {
        healUsed = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene($"rest_room_b{biomeId}");
    }

    uint HealAmount(float modifier) => (uint)(GameState.Run.Expedition.Player.maxHealth * modifier);
    uint FreeHealAmount() => HealAmount(freeHealModifier);
    uint PaidHealAmount() => HealAmount(paidHealModifier);
    public uint FreeHealRestores()
    {
        var player = GameState.Run.Expedition.Player;
        return Math.Min(FreeHealAmount(), player.maxHealth - player.currentHealth);
    }
    public uint PaidHealRestores()
    {
        var player = GameState.Run.Expedition.Player;
        return Math.Min(PaidHealAmount(), player.maxHealth - player.currentHealth);
    }

    public void FreeHeal()
    {
        if (healUsed)
            return;

        GameState.Run.Expedition.Player.currentHealth.Value += FreeHealAmount();
        
        healUsed = true;
        Debug.Log("Free heal used");
    }

    public void PaidHeal()
    {
        if (healUsed) return;

        var player = GameState.Run.Expedition.Player;

        if (!player.HasEnoughMaterials(PaidHealCost))
            return;

        player.LoseMaterials(PaidHealCost);
        player.currentHealth.Value += PaidHealAmount();

        healUsed = true;
        Debug.Log("Paid heal used");
    }

    public void Exit()
    {
        GameState.Run.Expedition.exitNode();
    }
}