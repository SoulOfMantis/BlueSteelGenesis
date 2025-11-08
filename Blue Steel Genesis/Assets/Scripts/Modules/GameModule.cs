using UnityEngine;


namespace BlueSteelGenesis.Modules
{
    /// <summary>
    /// класс модуля
    /// </summary>
    public abstract class GameModule
    {
        public TriggerType triggerType;

        public abstract void Effect(BlueSteelGenesis.Character.Character user, Vector3Int pos);

        public virtual void OnTrigger(BlueSteelGenesis.Character.Character user, Vector3Int pos)
        {
            Effect(user, pos);
        }

        public virtual void Initialize()
        {
            Debug.Log($"Module {GetType().Name} initialized");
        }


        public virtual void Execute()
        {
            Debug.Log("Execute called - override this method for proper functionality");
        }
    }
}