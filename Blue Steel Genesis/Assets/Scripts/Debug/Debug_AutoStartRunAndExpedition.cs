using UnityEngine;

public class Debug_AutoStartExpedition : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isPlaying)
        {
            GameState.startGameRun();
            GameState.Run.startExpedition(1);
        }
    }
}
