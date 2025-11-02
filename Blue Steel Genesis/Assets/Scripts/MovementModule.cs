using UnityEngine;

public class MovementModule : Module
{
    private Transform playerTransform;
    private float stepDistance = 1.0f;

    
    public MovementModule(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    
    public MovementModule(Transform playerTransform, float stepDistance)
    {
        this.playerTransform = playerTransform;
        this.stepDistance = stepDistance;
    }

    
    public override void Execute()
    {
        if (!isActive || playerTransform == null)
            return;
        playerTransform.Translate(0, 0, stepDistance);
        Debug.Log($"Игрок перемещен на {stepDistance} единиц. Новая позиция: {playerTransform.position}");
    }
    public override void Initialize()
    {
        Debug.Log("Модуль движения выполнен");
    }
}