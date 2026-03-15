using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_MenuButtonsForFightTest : MonoBehaviour
{
    public static void returnToMap() =>
        SceneManager.LoadScene("ExpeditionMapTest_usingGameState");

    public static void restartExpedition() {
        GameState.Run.endExpedition();
        GameState.Run.startExpedition(1);
        returnToMap();
    }
}
