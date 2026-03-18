using System;
using System.IO;
using UnityEngine;

public static class GameState
{
    private static readonly string SeedFilePath = "game_seed.txt";
    public static Expedition CurrentExpedition => Run?.Expedition;

    public static void startGameRun(int? seed = null)
    {
        if (Run != null)
        {
            Debug.LogWarning("Попытка начать новый забег до окончания предыдущего");
            return;
        }

        int actualSeed;
        if (seed.HasValue)
        {
            actualSeed = seed.Value;
            File.WriteAllText(SeedFilePath, actualSeed.ToString());
        }
        else
        {
            if (File.Exists(SeedFilePath))
            {
                string content = File.ReadAllText(SeedFilePath);
                if (int.TryParse(content, out actualSeed))
                {
                    Debug.Log($"Загружен сохранённый сид: {actualSeed}");
                }
                else
                {
                    actualSeed = generateRandomSeed();
                }
            }
            else
            {
                actualSeed = generateRandomSeed();
            }
        }
        Run = new(actualSeed);
        Run.start();
    }


    public static void endGameRun()
    {
        Run = null;
    }

    public static GameRun Run { get; private set; } = null;



    private static int generateRandomSeed()
    {
        byte[] seed_bytes = new byte[sizeof(int)];
        System.Security.Cryptography.RandomNumberGenerator.Fill(seed_bytes);
        int seed = BitConverter.ToInt32(seed_bytes);

        File.WriteAllText(SeedFilePath, seed.ToString());
        return seed;
    }
}
