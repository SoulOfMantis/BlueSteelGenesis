using System;
using UnityEngine;

public class ActionContext
{
    public Character actor { get; }
    public GameModule module { get; }
    public Vector3Int targetPosition { get; }
    public Entity targetEntity { get; }
    private object actionData;

    public ActionContext(Character actor, GameModule module, Vector3Int targetPosition)
    {
        this.actor = actor;
        this.module = module;
        this.targetPosition = targetPosition;
        targetEntity = Entity.tracker.FindEntityAtPosition(targetPosition);
    }

    public ActionContext WithActionData<T>(T actionData)
    {
        ActionContext ctx = (ActionContext)MemberwiseClone();
        ctx.actionData = actionData;
        return ctx;
    }

    public T GetActionData<T>() where T : class => actionData as T;
    public T? GetActionData<T>(int _ = 0) where T : struct =>
        actionData is T data ? data : null;
}

public static class ActionContextExtension
{
    public static void ThrowIfIncomplete(this ActionContext ctx)
    {
        if (ctx == null)
            throw new ArgumentNullException();
        if (ctx.actor == null)
            throw new InvalidOperationException("actor is not set");
        if (ctx.module == null)
            throw new InvalidOperationException("module is not set");
    }
}
