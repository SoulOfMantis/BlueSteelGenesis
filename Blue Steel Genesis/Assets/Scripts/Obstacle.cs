using UnityEngine;
using UnityEngine.UIElements;
using BlueSteelGenesis.Character_Modules;

public class Obstacle : MonoBehaviour
{
    public Vector3Int Position
    {
        get => position_;
        protected set
        {
            // TODO: adjust transform
            position_ = value;
        }
    }
    private Vector3Int position_;
}
