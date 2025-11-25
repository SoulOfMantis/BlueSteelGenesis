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
            if (inUse && Input.GetMouseButtonDown(0))
            {
                Vector3Int cell = Vector3Int.zero; //заглушка
                                                   //Vector3Int cell = SceneTracker.getCellByScreenPosition(Input.mousePosition);

                //TOD: use 
                //worldPoint = Camera.main.ScreenToWorldPoint(MousePosition);  tileMap.WorldToCell(worldPoint);
                //MousePosition -- добавить z-координату, равную расстоянию от камеры до поля (пока можно захардкодить), в остальном полученный вектор
                if (player.getCellsInRangeForModule(n).Contains(cell))
                {
                    player.useActiveModule(n, cell);
                }
                else inUse = false;
            }
        }


        public void toggleSkill()
        {
            inUse = !inUse;

        }
    }

}
