using UnityEngine;
public class Obstacle : MonoBehaviour
{
    public Vector3Int Position
    {
        get => position_;
        protected set
        {
            transform.position = Character.tracker.CellToWorld(value);
            position_ = value;
        }
    }
    private Vector3Int position_;
}
