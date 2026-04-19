using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeTracker : MonoBehaviour
{
    public List<Character> characters = new();
    public GameObject initiativeEntryPrefab;
    public Transform contentParent;
    private Dictionary<Character, InitiativeEntry> entries = new();
    Color highlightColor = Color.yellow;
    Color basicColor = Color.orange;
    Color takingTurnColor = Color.aquamarine;

    int currentCharacterIndex = -1;
    public void AddCharacter(Character c)
    {
        if ((c != null) && !(characters.Contains(c)))
        {
            int insert_index = characters.BinarySearch(c,
                Comparer<Character>.Create(
                    (ch1, ch2) => -ch1.Initiative.CompareTo(ch2.Initiative)));
            if (insert_index < 0)
                insert_index = ~insert_index;

            characters.Insert(insert_index, c);
            if (insert_index <= currentCharacterIndex)
                ++currentCharacterIndex;
            updateCharacterTooltips();
            if (currentCharacterIndex >= 0)
                StartCoroutine(TaskCoro.Make(c.startBattle()));

            Debug.Log($"Added {c.name}");
        }
    }
    public void RemoveCharacter(Character charact)
    {
        if (characters.Contains(charact))
        {
            if (characters.IndexOf(charact) <= currentCharacterIndex)
                currentCharacterIndex = (currentCharacterIndex - 1 + characters.Count) % characters.Count;
            characters.Remove(charact);
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
        entries[c].GetComponent<Image>().color = color;
    }
    public void HighlightCharacterInInitiative(Character c) => HighlightCharacterInInitiative(c, highlightColor);
    public void UnhighlightCharacterInInitiative(Character c)
    {
        if (characters[currentCharacterIndex] == c)
            HighlightCharacterInInitiative(c, takingTurnColor);
        else HighlightCharacterInInitiative(c, basicColor);
    }
    public bool isAlive(Character c) =>
        characters.Contains(c);
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
            HighlightCharacterInInitiative(characters[currentCharacterIndex], takingTurnColor);
            Debug.Log($"Сейчас ход {characters[currentCharacterIndex].GetType().Name}");
            StartCoroutine(TaskCoro.Make(characters[currentCharacterIndex].startTurn()));
        }
    }
    private void updateCharacterTooltips()
    {
        foreach (var entry in entries)
        {
            entry.Value.gameObject.SetActive(false);
            Destroy(entry.Value.gameObject);
        }
        entries.Clear();
        for (int i = 0; i < characters.Count; i++)
            createCharacterTooltipTrigger(characters[i]);
    }
    void createCharacterTooltipTrigger(Character c)
    {
        var go = Instantiate(initiativeEntryPrefab, contentParent);
        var entry = go.GetComponent<InitiativeEntry>();
        entry.Setup(c);
        entries[c] = entry;
        HighlightCharacterInInitiative(c, Color.orange);
    }
    public void StartBattle()
    {
        foreach (Character c in characters)
            StartCoroutine(TaskCoro.Make(c.startBattle()));
        updateCharacterTooltips();
        StartNextTurn();
    }

    void Start()
    {
        StartBattle();
    }
}
