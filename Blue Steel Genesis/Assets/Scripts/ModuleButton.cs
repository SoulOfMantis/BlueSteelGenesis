using UnityEngine;
using UnityEngine.UI;

namespace BlueSteelGenesis.Character_Modules
{
    public class ModuleButton : MonoBehaviour
    {
        public PlayerCharacter player;
        private Button button;
        public int n;
        private bool inUse = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            button = gameObject.GetComponent<Button>();
            if (!player.DoesModuleExist(n))
                button.gameObject.SetActive(false);
            else if (player.IsModulePassive(n))
                button.interactable = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        Vector3Int getCellByClick()
        {
            //...
            return Vector3Int.zero;
        }

        public void activateModule()
        {
            inUse = !inUse;
            if (inUse)
            {
                var pos = getCellByClick();
                //if (false) return; //invalid position
                if (player.useActiveModule(n, pos))
                {
                    //use animation
                }
                else
                {
                    //failed to use animation
                }
            }
        }
    }

}
