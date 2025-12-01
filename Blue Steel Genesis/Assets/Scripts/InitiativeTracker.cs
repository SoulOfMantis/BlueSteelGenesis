using BlueSteelGenesis.Character_Modules;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Character = BlueSteelGenesis.Character_Modules.Character;
public class InitiativeTracker : MonoBehaviour
{
    public List<Character> characters;

    int currentCharacterIndex = 0;
    public void AddCharacter(Character charact)
    {
        if ((charact != null) && !(characters.Contains(charact)))
        {
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


    public void StartNextTurn()
    {
        if (!CheckVictory())
        {
            currentCharacterIndex = (currentCharacterIndex) % characters.Count;


            Debug.Log($"Начинается ход {characters[currentCharacterIndex].name}");
            characters[currentCharacterIndex].startTurn();
            currentCharacterIndex++;
        }
        else { PlayerCharacter.Victory(); }

    }

    public void StartBattle() 
    {
        characters.Sort((c1, c2) => (c1.initiative.CompareTo(c2.initiative)));
        characters.ForEach(c => c.startBattle());
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
