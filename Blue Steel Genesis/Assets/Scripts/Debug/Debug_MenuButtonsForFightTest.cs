using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_MenuButtonsForFightTest : MonoBehaviour
{
    public static void returnToMap() =>
        SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
}
