using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InitiativeTracker : MonoBehaviour
{
    int currentCharacterIndex = 0;
    int i = 0;
    private void FindAllCharacters()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();
        characters.AddRange(allCharacters);

        Debug.Log($"Найдено персонажей: {characters.Count}");
    }

    public void CharacterEndedTurn()
    {
        Debug.Log($"{characters[currentCharacterIndex].name} закончил ход");
        characters[currentCharacterIndex].endTurn();
        StartNextTurn();
    }

    private void StartNextTurn()
    {

        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

        Debug.Log($"Начинается ход {characters[currentCharacterIndex].name}");
        characters[currentCharacterIndex].startTurn();
    }

    public List<Character> characters;

    void Start()
    {
        Character.Tracker = this;
        FindAllCharacters();

        if (characters.Count > 0)
        {
            StartNextTurn();
        }

    }

    void Update()
    {

        i++;
        if (i >= 500)
        {
            i = 0;
            CharacterEndedTurn();
        }

    }
}
