using System;
using System.IO;
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
        Run.AutoEndPlayerTurn = AutoEndPlayerTurn;
        Run.start();
    }

    public static void saveGameRun()
    {
        string serialized = JsonUtility.ToJson(Run);
        File.WriteAllText(SaveFilePath, serialized);
    }
    public static bool loadGameRun()
    {
        try {
            string serialized = File.ReadAllText(SaveFilePath);
            Run = JsonUtility.FromJson<GameRun>(serialized);
            return true;
        }
        catch (Exception ex) {
            Debug.LogError(ex);
            return false;
        }
    }
    public static bool saveFileExists() =>
        File.Exists(SaveFilePath);

    public static void endGameRun()
    {
        Run = null;
    }
    public static bool AutoEndPlayerTurn;
    public static GameRun Run { get; private set; } = null;
    private const string SaveFilename = "save.json";
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFilename);

    private static int generateRandomSeed() {
        byte[] seed_bytes = new byte[sizeof(int)];
        System.Security.Cryptography.RandomNumberGenerator.Fill(seed_bytes);
        return BitConverter.ToInt32(seed_bytes);
    }
}
