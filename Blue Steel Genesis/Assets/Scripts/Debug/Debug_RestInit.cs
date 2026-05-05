using UnityEngine;

public class Debug_RestInit : MonoBehaviour
{
    [SerializeField] private bool enableDebugInit = true;

    private void Awake()
    {
        if (!enableDebugInit)
            return;

        if (GameState.Run != null && GameState.Run.Expedition != null)
            return;

        Debug.Log("Debug Initialization: creating temporary game state for Rest testing.");

        GameState.startGameRun(12345);
        GameState.Run.startExpedition(1);

        var player = GameState.Run.Expedition.Player;
        player.maxHealth = 100;
        player.currentHealth.Value = 30;
        player.GiveMaterials(500);
        player.maxEnergy = 10;
        player.modules = new System.Collections.Generic.List<GameModule>{
            new DefaultAdaptiveTEST_ONLY(),
            new MechanicStinger(),
            new BasicMovement(),
            new BasicAttack()
        };

        GameState.Run.Expedition.Rest = new Rest(1); 
        GameState.Run.Expedition.Rest.Trigger();
    }
}