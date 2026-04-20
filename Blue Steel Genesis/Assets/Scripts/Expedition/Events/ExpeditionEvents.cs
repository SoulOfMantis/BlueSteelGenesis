using Map;
using UnityEngine;

public partial class Expedition
{
    private bool isInEvent = false;

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
                CombatSystem.TriggerNormalEncounter();
                break;
            case Node.ELITE_ENEMY:
                CombatSystem.TriggerEliteEncounter();
                break;
            case Node.BOSS:
                CombatSystem.TriggerBossEncounter();
                break;
            case Node.SHOP:
                Shop.TriggerShop();
                break;
            case Node.REST:
                // логика отдыха
                break;
            case Node.TREASURE:
                // логика сокровища
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

        EventData eventData = EventManager.GetRandomEventForStage();
        CurrentEventHolder.Event = eventData;
        UnityEngine.SceneManagement.SceneManager.LoadScene(eventData.sceneName);
    }
}