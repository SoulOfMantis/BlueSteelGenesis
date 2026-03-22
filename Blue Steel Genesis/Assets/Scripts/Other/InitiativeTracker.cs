using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InitiativeTracker : MonoBehaviour
{
    public List<Character> characters = new();
    private Dictionary<Character, GameObject> characterTooltipsTriggers = new();

    int currentCharacterIndex = -1;
    public void AddCharacter(Character charact)
    {
        if ((charact != null) && !(characters.Contains(charact)))
        {
            int insert_index = characters.BinarySearch(charact,
                Comparer<Character>.Create(
                    (ch1, ch2) => -ch1.Initiative.CompareTo(ch2.Initiative)));
            if (insert_index < 0)
                insert_index = ~insert_index;

            characters.Insert(insert_index, charact);
            if (insert_index <= currentCharacterIndex)
                ++currentCharacterIndex;
            createCharacterTooltipTrigger(charact);
            updateCharacterTooltips();
            StartCoroutine(TaskCoro.Make(charact.startBattle()));

            Debug.Log($"Added {charact.name}");
        }
    }
    public void RemoveCharacter(Character charact)
    {
        if (characters.Contains(charact))
        {
            if (characters.IndexOf(charact) <= currentCharacterIndex)
                currentCharacterIndex = (currentCharacterIndex - 1 + characters.Count) % characters.Count;
            characters.Remove(charact);
            Destroy(characterTooltipsTriggers[charact]);
            characterTooltipsTriggers.Remove(charact);
            updateCharacterTooltips();
        }
    }

    public PlayerCharacter getPlayer() =>
        characters.Find(c => c is PlayerCharacter) as PlayerCharacter;

    public bool CheckVictory()
    {
        return characters.All(c => c is PlayerCharacter || c is Ally);
    }

    public bool CheckDefeat()
    {
        return !characters.Exists(c => c is PlayerCharacter);
    }

    public void HighlightCharacterInInitiative(Character c, Color color)
    {
        characterTooltipsTriggers[c].GetComponent<TextMeshProUGUI>().color = color;
    }
    public void HighlightCharacterInInitiative(Character c) => HighlightCharacterInInitiative(c, Color.yellow);
    public void UnhighlightCharacterInInitiative(Character c)
    {
        if (characters[currentCharacterIndex] == c)
            characterTooltipsTriggers[c].GetComponent<TextMeshProUGUI>().color = Color.blue;
        else characterTooltipsTriggers[c].GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    public void StartNextTurn()
    {
        Debug.Log($"StartNextTurn");
        if (CheckVictory())
            StartCoroutine(TaskCoro.Make(Entity.tracker.getPlayer().Victory()));
        else if (!CheckDefeat())
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            //снимаем выделение с походившего, такой страшный индекс нужен, чтобы не выйти за пределы списка
            UnhighlightCharacterInInitiative(characters[(currentCharacterIndex-1+characters.Count)%characters.Count]); 
            HighlightCharacterInInitiative(characters[currentCharacterIndex], Color.blue);
            Debug.Log($"Сейчас ход {characters[currentCharacterIndex].GetType().Name}");
            StartCoroutine(TaskCoro.Make(characters[currentCharacterIndex].startTurn()));
        }
    }
    private void updateCharacterTooltips()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            var c = characters[i];
            var ctt = characterTooltipsTriggers[c];
            ctt.GetComponent<TextMeshProUGUI>().text = $"{i+1}    {c.Name}";
            ctt.transform.position = transform.position + new Vector3(0, -2 * i - 0.6f); //Hardcoded for now
        }
    }
    void createCharacterTooltipTrigger(Character c)
    {
        characterTooltipsTriggers[c] = new GameObject($"{c.Name}");
        var ctt = characterTooltipsTriggers[c];
        ctt.AddComponent<CharacterTooltipTrigger>().character = c;
        ctt.AddComponent<TextMeshProUGUI>().enableAutoSizing = true;
        ctt.transform.SetParent(transform);
        ctt.transform.localScale = new(1, 1);
    }
    public void StartBattle()
    {
        updateCharacterTooltips();
        StartNextTurn();
    }

    void Start()
    {
        StartBattle();
    }
}
