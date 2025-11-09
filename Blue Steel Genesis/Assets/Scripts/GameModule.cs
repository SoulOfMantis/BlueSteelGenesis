using UnityEngine;



namespace BlueSteelGenesis.Character_Modules
{
    /// <summary>
    /// класс модуля
    /// </summary>
    public abstract class GameModule
    {
        public abstract void Effect(Character user, Vector3Int pos);

        public virtual void Initialize()
        {
            Debug.Log($"Module {GetType().Name} initialized");
        }

    }
}