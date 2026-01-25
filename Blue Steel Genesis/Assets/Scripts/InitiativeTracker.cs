using BlueSteelGenesis.Character_Modules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character = BlueSteelGenesis.Character_Modules.Character;
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


    public IEnumerator StartNextTurn()
    {
        Debug.Log($"StartNextTurn");
        while (!CheckDefeat() && !CheckVictory())
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

            Debug.Log($"Сейчас ход {characters[currentCharacterIndex].GetType().Name}");
            yield return TaskCoro.Make(characters[currentCharacterIndex].startTurn());
        }
        if (CheckVictory()) Character.tracker.getPlayer().Victory();
    }

    public void StartBattle() 
    {
        characters.Sort((c1, c2) => (c2.Initiative.CompareTo(c1.Initiative)));
        characters.ForEach(c => c.startBattle());
        StartCoroutine(nameof(StartNextTurn));
    }

    void Start()
    {
        StartBattle();
    }

    void Update()
    {


    }
}
