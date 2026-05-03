using UnityEngine;

public class SaveTest : MonoBehaviour {
    public void StartNewExpedition() {
        GameState.startGameRun();
        GameState.Run.startExpedition(1);
        GameState.Run.Expedition.showExpeditionMap();
    }
    public void LoadExpedition() {
        if (GameState.loadGameRun())
            GameState.Run.Expedition.showExpeditionMap();
    }
}