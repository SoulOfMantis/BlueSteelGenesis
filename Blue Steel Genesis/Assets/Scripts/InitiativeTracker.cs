using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InitiativeTracker : MonoBehaviour
{
    public List<Character> characters;

    int currentCharacterIndex = 0;
    public void AddCharacter(Character charact)
    {
        characters.Add(charact);
    }
    public void RemoveCharacter(Character character)
    {
        characters.Remove(character);
    }




    public void StartNextTurn()
    {

        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

        Debug.Log($"Начинается ход {characters[currentCharacterIndex].name}");
        characters[currentCharacterIndex].startTurn();

    }


    void Start()
    {
        Character.Tracker = this;

        StartNextTurn();

            
        
    }

    void Update()
    {


    }
}
