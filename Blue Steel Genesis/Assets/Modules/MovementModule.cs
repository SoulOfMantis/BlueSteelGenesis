using UnityEngine;

/// <summary>
/// модуль движени€
/// </summary>
public class MovementModule : Module
{
    /// <summary>
    /// Transform игрока дл€ управлени€ его позицией и движением
    /// </summary>
    private Transform playerTransform;
    /// <summary>
    /// длина шага
    /// </summary>
    private float stepDistance = 1.0f;

    /// <summary>
    ///  онструктор без возможности указать дистанцию шага
    /// </summary>
    /// <param name="playerTransform"></param>
    public MovementModule(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    /// <summary>
    ///  онструктор с возможностью указать дистанцию шага
    /// </summary>
    /// <param name="playerTransform"></param>
    /// <param name="stepDistance"></param>
    public MovementModule(Transform playerTransform, float stepDistance)
    {
        this.playerTransform = playerTransform;
        this.stepDistance = stepDistance;
    }

    /// <summary>
    ///  –еализаци€ выполнени€ движени€
    /// </summary>
    public override void Execute()
    {
        if (!isActive || playerTransform == null)
            return;
        playerTransform.Translate(0, 0, stepDistance);
        Debug.Log($"»грок перемещен на {stepDistance} единиц. Ќова€ позици€: {playerTransform.position}");
    }
    public override void Initialize()
    {
        Debug.Log("ћодуль движени€ выполнен");
    }
}