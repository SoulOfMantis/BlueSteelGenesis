using UnityEngine;

public class AutoEndTurn : MonoBehaviour
{
    public void SetAutoEndPlayerTurn(bool value) => GameState.AutoEndPlayerTurn = value;
}
