using UnityEngine;

public class GODMODE : MonoBehaviour
{
    public void ActivateGodmode()
    {
        var player = GameState.Run.Expedition.Player;
        player.maxHealth = 1000;
        player.currentHealth.Value = player.maxHealth;
        player.modules[1] = new OverpoweredStinger();
    }
}
