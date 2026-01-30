using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class InitiativeTracker : MonoBehaviour
{
    public List<Character> characters = new();

    int currentCharacterIndex = -1;
    public void AddCharacter(Character charact)
    {
        if ((charact != null) && !(characters.Contains(charact)))
        {
            Debug.Log($"Added {charact.name}");
            characters.Add(charact);
        }
    }
    public void RemoveCharacter(Character charact)
    {
        if (characters.Contains(charact))
        {
            characters.Remove(charact);
        }
    }
    public bool CheckVictory()
    {
        return characters.All(c => c is PlayerCharacter);
    }

    public bool CheckDefeat()
    {
        return !characters.Exists(c => c is PlayerCharacter);
    }

    public bool isAlive(Character c)
    { return characters.Contains(c); }
    public void StartNextTurn()
    {
        Debug.Log($"StartNextTurn");
        if (CheckVictory())
            StartCoroutine(TaskCoro.Make(Character.tracker.getPlayer().Victory()));
        else if (!CheckDefeat())
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

            Debug.Log($"Сейчас ход {characters[currentCharacterIndex].GetType().Name}");
            StartCoroutine(TaskCoro.Make(characters[currentCharacterIndex].startTurn()));
        }
    }

    public void StartBattle()
    {
        characters.Sort((c1, c2) => (c2.Initiative.CompareTo(c1.Initiative)));
        characters.ForEach(c => StartCoroutine(TaskCoro.Make(c.startBattle())));
        StartNextTurn();
    }

    void Start()
    {
        StartBattle();
    }

    void Update()
    {


    }
}
