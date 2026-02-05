using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class InitiativeTracker : MonoBehaviour
{
    private TMP_Text initiativeOrder;
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


    public void StartNextTurn()
    {
        Debug.Log($"StartNextTurn");
        if (CheckVictory())
            StartCoroutine(TaskCoro.Make(Character.tracker.getPlayer().Victory()));
        else if (!CheckDefeat())
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            updateInitiativeOrder();

            Debug.Log($"Сейчас ход {characters[currentCharacterIndex].GetType().Name}");
            StartCoroutine(TaskCoro.Make(characters[currentCharacterIndex].startTurn()));
        }
    }
    private void updateInitiativeOrder()
    {
        initiativeOrder.text = "";
        for (int i = 0; i < characters.Count; i++)
        {
            string line = (characters[i].Name + " " + characters[i].Initiative);
            if (i == currentCharacterIndex)
                line = "<color=yellow>" + line + "<color=white>";
            line += "\n";
            initiativeOrder.text += line;
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
        initiativeOrder = GetComponentInChildren<TMP_Text>();
        StartBattle();
    }

    void Update()
    {


    }
}
