using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BlueSteelGenesis.Character_Modules
{
    public class ModuleButton : MonoBehaviour
    {
        private PlayerCharacter player;
        private Button button;
        private InputAction gridClickAction;
        public int connectedModuleIndex;
        private bool inUse = false;
        
        private static UnityEvent resetSelection = new();
        
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

            gridClickAction = new InputAction(binding: "<Mouse>/leftButton");
            gridClickAction.started += handleGridClick;
            gridClickAction.Enable();

            resetSelection.AddListener(deselect);
        }

        private void OnDestroy() =>
            resetSelection.RemoveListener(deselect);

        public void handleGridClick(InputAction.CallbackContext _) {
            Vector3Int cell = Character.tracker.GetCellByScreenPosition(Input.mousePosition);
            if (!inUse || cell == new Vector3Int(-1, -1, -1) || !player.GetModulePositions(connectedModuleIndex).Contains(cell))
                return;
            toggleSkill();
            player.useActiveModule(connectedModuleIndex, cell);
        }

        public void deselect() {
            if (inUse) toggleSkill();
        }

        public void buttonInteractableManaging()
        {
            if (player.myTurn) button.interactable = true;            
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
                Character.tracker.ClearHighlights(player.GetModulePositions(connectedModuleIndex));
                inUse = false;
            }
            else if (player.canUseModule(connectedModuleIndex))
            {
                //highlight tiles
                resetSelection.Invoke();
                Character.tracker.HighlightCells(player.GetModulePositions(connectedModuleIndex));
                inUse = true;
            }
        }
    }
}
