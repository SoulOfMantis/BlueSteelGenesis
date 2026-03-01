using System;
using UnityEngine;

public static class GameState
{
    public static void startGameRun(int? seed = null)
    {
        if (Run != null) {
            Debug.LogWarning("Попытка начать новый забег до окончания предыдущего");
            return;
        }
        Run = new(seed ?? generateRandomSeed());
        Run.start();
    }

    public static void endGameRun()
    {
        Run = null;
    }

    public static GameRun Run { get; private set; } = null;



    private static int generateRandomSeed() {
        byte[] seed_bytes = new byte[sizeof(int)];
        System.Security.Cryptography.RandomNumberGenerator.Fill(seed_bytes);
        return BitConverter.ToInt32(seed_bytes);
    }
}
