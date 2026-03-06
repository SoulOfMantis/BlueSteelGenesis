using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CharacterVisualHandler : VisualHandlerBase
{
    [System.Serializable]
    public struct AnimationClipRef
    {
        public string animationName;
        public AnimationClip clip;
        public float transitionDuration;
    }

    [Header("Character animations")]
    [SerializeField] private AnimationClipRef idleAnimation;
    [SerializeField] private AnimationClipRef walkAnimation;
    [SerializeField] private AnimationClipRef attackAnimation;
    [SerializeField] private AnimationClipRef hurtAnimation;
    [SerializeField] private AnimationClipRef deathAnimation;

    [Header("Effect")]
    [SerializeField] private AnimationClipRef healEffect;

    //[Header("Floating text")]
    //[SerializeField] private GameObject floatingTextPrefab;
    //[SerializeField] private Transform floatingTextSpawnPoint;

    private string currentAnimation;

    public async Task PlayWalkAnimation(Vector3Int direction)
    {
        PlayAnimation(walkAnimation.animationName);

        await Task.Delay((int)(walkAnimation.transitionDuration * 1000));
    }

    public async Task PlayAttackAnimation(Vector3Int target)
    {
        PlayAnimation(attackAnimation.animationName);
        LookAt(target);

        await Task.Delay((int)(attackAnimation.transitionDuration * 1000));
    }

    public async Task PlayHurtAnimation(int amount)
    {
        PlayAnimation(hurtAnimation.animationName);

        //ShowFloatingText($"-{amount}", Color.red);

        await Task.Delay((int)(hurtAnimation.transitionDuration * 1000));

    }

    public async Task PlayDeathAnimation()
    {
        PlayAnimation(deathAnimation.animationName);

        await Task.Delay((int)(deathAnimation.transitionDuration * 1000));
    }

    public async Task PlayHealingAnimation(int amount)
    {
        PlayAnimation(healEffect.animationName);

        await Task.Delay((int)(healEffect.transitionDuration * 1000));
        //ShowFloatingText($"+{amount}", Color.green);
    }

    // Заставить персонажа смотреть в направление клетки
    private void LookAt(Vector3Int target)
    {
        Vector3 direction = (Character.tracker.CellToWorld(target) - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                spriteRenderer.flipX = false;
            else 
                spriteRenderer.flipX = true;
        }
    }


    // Частично работает, пока закомментировано
/*    private void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab == null) return;

        Vector3 spawnPos = floatingTextSpawnPoint != null
            ? floatingTextSpawnPoint.position : transform.position;

        GameObject textObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);

        TMP_Text tmp = textObj.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
        }

        Destroy(textObj, 1.5f);
        Destroy(textObj, 1.5f);
    }*/
}