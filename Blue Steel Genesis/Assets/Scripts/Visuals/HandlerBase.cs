using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class VisualHandlerBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    // Инициализирует обработчик спрайтов и анимаций
    protected virtual void Inititate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Задаёт спрайт объекта
    public virtual void SetSprite(Sprite sprite)
    {
        if (spriteRenderer  != null)
            spriteRenderer.sprite = sprite;
    }

    // Проиграть заданную анимацию
    public virtual void PlayAnimation(string animationName)
    {
        if (animator  != null)
            animator.Play(animationName);
    }

    // Задать триггер
    public virtual void SetTrigger(string triggerName)
    {
        if (animator != null)
            animator.SetTrigger(triggerName);
    }

    // Задать значение параметру
    public virtual void SetBool(string parameterName, bool value)
    {
        if (animator != null)
            animator.SetBool(parameterName, value);
    }
}