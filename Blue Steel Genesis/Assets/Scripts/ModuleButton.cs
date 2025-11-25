using UnityEngine;
using UnityEngine.UI;

namespace BlueSteelGenesis.Character_Modules
{
    public class ModuleButton : MonoBehaviour
    {
        private PlayerCharacter player;
        private Button button;
        public int n;
        private bool inUse = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>(); //Will cause a crash if there's no active player game object, shouldn't let this happen
            button = gameObject.GetComponent<Button>();
            if (!player.DoesModuleExist(n))
                gameObject.SetActive(false);
            this.enabled = player.IsModuleActive(n);
            buttonInteractableManaging();
        }

        // Update is called once per frame
        void Update()
        {
            buttonInteractableManaging();
            if (inUse && Input.GetMouseButtonDown(0))
            {
                Vector3Int cell = Vector3Int.zero; //заглушка

                //Vector3Int cell = SceneTracker.sceneTracker.getCellByScreenPosition(Input.mousePosition);


                if (player.getCellsInRangeForModule(n).Contains(cell))
                {
                    player.useActiveModule(n, cell);
                }
                inUse = false;
            }
        }

        void buttonInteractableManaging()
        {
            button.interactable = player.myTurn;
        }

        public void toggleSkill()
        {
            inUse = !inUse;
            if (inUse)
            {
                //toggle tiles to be higlighted
            }
            else
            {
                //toggle tiles to not be highlighted
            }
        }
    }

}
