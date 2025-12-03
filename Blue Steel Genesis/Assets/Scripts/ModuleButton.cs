using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueSteelGenesis.Character_Modules
{
    public class ModuleButton : MonoBehaviour
    {
        private PlayerCharacter player;
        private Button button;
        public int connectedModuleIndex;
        private bool inUse = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>(); //Will cause a crash if there's no active player game object, shouldn't let this happen
            button = gameObject.GetComponent<Button>();
            if (!player.doesModuleExist(connectedModuleIndex))
                gameObject.SetActive(false);
            button.interactable = false;
            enabled = player.isActive(connectedModuleIndex);
            PlayerCharacter.activeModuleButtons.Add(this);
            button.GetComponentInChildren<TMP_Text>().text = player.getmoduleName(connectedModuleIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if (inUse && Input.GetMouseButtonDown(0))
            {
                Vector3Int cell = Character.tracker.GetCellByScreenPosition(Input.mousePosition);
                if (cell != new Vector3Int(-1, -1, -1))
                {
                    player.useActiveModule(connectedModuleIndex, cell);
                }
                else Debug.Log("Impossible position!");
                inUse = false;
            }
        }

        public void buttonInteractableManaging()
        {
            if (player.myTurn)  button.interactable = true;            
            else
            {
                button.interactable = false;
                if (inUse) toggleSkill();
            }
        }

        public void toggleSkill()
        {
            if (inUse)
            {
                //unhighlight tiles
                inUse = false;
            }
            else if (player.canUseModule(connectedModuleIndex))
            {
                //highlight tiles
                inUse = true;
            }
        }
    }

}
