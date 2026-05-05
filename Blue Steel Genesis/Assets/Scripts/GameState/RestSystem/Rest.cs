using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Rest
{
    [SerializeField]
    private uint biomeId;

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

    public void FreeHeal()
    {
        if (healUsed)
            return;

        var player = GameState.Run.Expedition.Player;
        uint healAmount = (uint)(player.maxHealth * 0.3f);
        player.currentHealth.Value = Math.Min(player.currentHealth.Value + healAmount, player.maxHealth);
        
        healUsed = true;
        Debug.Log("Free heal used");
    }

    public void PaidHeal()
    {
        if (healUsed) return;

        var player = GameState.Run.Expedition.Player;
        const uint cost = 50;

        if (!player.HasEnoughMaterials(cost))
            return;

        player.LoseMaterials(cost);
        player.currentHealth.Value = player.maxHealth;

        healUsed = true;
        Debug.Log("Paid heal used");
    }

    public void Exit()
    {
        GameState.Run.Expedition.exitNode();
    }
}