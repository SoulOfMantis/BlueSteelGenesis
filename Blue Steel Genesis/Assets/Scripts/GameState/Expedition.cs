using Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using HKDF = HKDF<System.Security.Cryptography.HMACSHA1>;
using static GameState;

public class Expedition
{
    public Expedition(Map.BiomeInfo biome)
    {
        Biome = biome;
        BiomeSeed = generateBiomeSeed(GameState.Run.GlobalSeed, biome.id);
    }

    public void start()
    {
        // TODO: handle player creation properly
        Player.modules = new List<GameModule>{
            new MechanicStinger(),
            new BasicMovement()
        };
        Player.maxHealth = 10;
        Player.maxEnergy = 3;
        Player.currentHealth = Player.maxHealth;
        Player.materials = 3;
        Player.money = 10;

        startNextStage();
    }

    public void startNextStage()
    {
        ++BiomeStage;

        LocalSeed = generateLocalSeed(
            GameState.Run.GlobalSeed,
            Biome.id, (uint)BiomeStage,
            GameState.Run.playerLivesCount,
            Array.Empty<byte>() //TODO: pass actual data
        );
        Map = global::Map.ExpeditionMap.generate(
            BiomeSeed, LocalSeed,
            Biome, (uint)BiomeStage
        );
        map_progress_ = new(Map);
        ModuleGen = new(LocalSeed);
    }

    public void displayMap(ExpeditionMapView view)
    {
        if (Map == null || map_progress_ == null)
            throw new InvalidOperationException("Невозможно отобразить карту до начала этапа");
        if (view != null)
            view.make(Map, map_progress_);
    }

    public int LocalSeed { get; private set; }
    public int BiomeSeed { get; private set; }

    public PlayerData Player { get; private set; } = new();
    public Map.ExpeditionMap Map { get; private set; } = null;
    public Map.BiomeInfo Biome { get; private set; }

    private ExpeditionMapProgressInfo map_progress_ = null;
    public int BiomeStage { get; private set; } = -1;



    private static int generateLocalSeed(int global_seed, uint biome_id, uint biome_stage, uint lives_count, byte[] ship_parts_data)
    {
        HKDF hkdf = new();
        hkdf.extract(null, BitConverter.GetBytes(global_seed));
        int seed = BitConverter.ToInt32(
            hkdf.expand(ArrayUtil.join(
                BitConverter.GetBytes(biome_id),
                BitConverter.GetBytes(biome_stage),
                BitConverter.GetBytes(lives_count),
                ship_parts_data
            ),
            sizeof(int)));
        return seed;
    }
    private static int generateBiomeSeed(int global_seed, uint biome_id)
    {
        HKDF hkdf = new();
        hkdf.extract(null, BitConverter.GetBytes(global_seed));
        int seed = BitConverter.ToInt32(
            hkdf.expand(BitConverter.GetBytes(biome_id), sizeof(int)));
        return seed;
    }
    public GameModule GetNextModule() => ModuleGen.GetNextModule();
    ModuleGenerator ModuleGen;





    private bool isInEvent = false;
    private EventData currentEvent;
    private Node pendingBattleType; 

    
    public void EnterNode(Vector2Int nodePos)
    {
        if (Map == null) return;

        Node nodeType;
        if (nodePos == Map.start_node_pos)
            nodeType = Node.START;
        else if (nodePos == Map.boss_node_pos)
            nodeType = Node.BOSS;
        else
            nodeType = Map.map[nodePos.y, nodePos.x];

 

        switch (nodeType)
        {
            case Node.EVENT:
                StartEvent();
                break;
            case Node.REGULAR_ENEMY:
            case Node.ELITE_ENEMY:
                StartBattle(nodeType);
                break;
            case Node.SHOP:
                StartShop();
                break;
            case Node.REST:
                break;
            case Node.TREASURE:
                break;
            default:
                Debug.LogWarning($"Неподдерживаемый тип узла: {nodeType}");
                break;
        }
    }


    private void StartEvent()
    {
        if (isInEvent) return;
        isInEvent = true;

        int eventSeed = LocalSeed + BiomeStage * 100 + (int)Biome.id;
        currentEvent = EventManager.GetRandomEventForBiome(Biome.id, eventSeed);

        GlobalEventStorage.CurrentEvent = currentEvent;

        UnityEngine.SceneManagement.SceneManager.LoadScene("EventScene");
    }


    private void StartBattle(Node enemyType)
    {
        //Запуск битвы
    }


    private void StartShop()
    {
        //запуск магазина
    }

  
    private void ReturnToMap()
    {
        //вернуться на карут
    }


    // Методы для обработки результатов (вызываются из других сцен)
    public void HandleEventOutcome(EventOutcome outcome)
    {
        isInEvent = false;
        switch (outcome)
        {
            case EventOutcome.Exit:
                ReturnToMap();
                break;
            case EventOutcome.EnterBattle:
                Node enemyType = (UnityEngine.Random.value < 0.5f) ? Node.REGULAR_ENEMY : Node.ELITE_ENEMY;
                StartBattle(enemyType);
                break;
            case EventOutcome.EnterShop:
                StartShop();
                break;
        }
    }

    public void HandleBattleOutcome(bool victory)
    {
        if (victory)
        {
            // Начислить награду за победу (ресурсы, предметы)
            // Например: Player.money += 10;
        }
        // В любом случае возвращаемся на карту
        ReturnToMap();
    }




}
