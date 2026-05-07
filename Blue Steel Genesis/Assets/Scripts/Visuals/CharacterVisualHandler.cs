using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterVisualHandler : VisualHandlerBase
{
    [System.Serializable]
    public struct AnimationClipRef
    {
        public string animationName;
        public AnimationClip clip;
        public float transitionDuration;
    }

    [Header("Base character animations")]
    [SerializeField] private AnimationClipRef idleAnimation;
    [SerializeField] private AnimationClipRef walkAnimation;
    [SerializeField] private AnimationClipRef attackAnimation;

    [Header("Health/Shield Animations")]
    [SerializeField] private AnimationClipRef hurtAnimation;
    [SerializeField] private AnimationClipRef deathAnimation;
    [SerializeField] private AnimationClipRef loseShieldAnimation;
    [SerializeField] private AnimationClipRef gainShieldAnimation;
    [SerializeField] private AnimationClipRef healAnimation;

    [Header("Turn/Battle Animations")]
    [SerializeField] private AnimationClipRef startBattleAnimation;
    [SerializeField] private AnimationClipRef endBattleAnimation;
    [SerializeField] private AnimationClipRef startTurnAnimation;
    [SerializeField] private AnimationClipRef endTurnAnimation;

    [Header("Special Animations")]
    [SerializeField] private AnimationClipRef summonAnimation;
    [SerializeField] private AnimationClipRef specialAnimation;



    //[Header("Floating text")]
    //[SerializeField] private GameObject floatingTextPrefab;
    //[SerializeField] private Transform floatingTextSpawnPoint;

    private string currentAnimation;

    public async Task PlaySpecialAnimation()
    {
        PlayAnimation(specialAnimation.animationName);

        await Task.Delay((int)(specialAnimation.transitionDuration * 1000));
    }

    public async Task PlayWalkAnimation(Vector3Int direction)
    {
        PlayAnimation(walkAnimation.animationName);
        LookAt(direction);

        await Task.Delay((int)(walkAnimation.transitionDuration * 1000));
    }

    public async Task PlayStartBattleAnimation()
    {
        PlayAnimation(startBattleAnimation.animationName);

        await Task.Delay((int)(startBattleAnimation.transitionDuration * 1000));
    }

    public async Task PlayEndBattleAnimation()
    {
        PlayAnimation(endBattleAnimation.animationName);

        await Task.Delay((int)(endBattleAnimation.transitionDuration * 1000));
    }

    public async Task PlayStartTurnAnimation()
    {
        PlayAnimation(startTurnAnimation.animationName);

        await Task.Delay((int)(startTurnAnimation.transitionDuration * 1000));
    }

    public async Task PlayEndTurnAnimation()
    {
        PlayAnimation(endTurnAnimation.animationName);

        await Task.Delay((int)(endTurnAnimation.transitionDuration * 1000));
    }

    public async Task PlayAttackAnimation(Vector3Int target)
    {
        PlayAnimation(attackAnimation.animationName);
        LookAt(target);

        await Task.Delay((int)(attackAnimation.transitionDuration * 1000));
    }

    public async Task PlayHurtAnimation(uint amount)
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

    public async Task PlayHealingAnimation(uint amount)
    {
        PlayAnimation(healAnimation.animationName);

        await Task.Delay((int)(healAnimation.transitionDuration * 1000));
        //ShowFloatingText($"+{amount}", Color.green);
    }

    public async Task PlayGainShieldAnimation(uint amount)
    {
        PlayAnimation(gainShieldAnimation.animationName);

        await Task.Delay((int)(gainShieldAnimation.transitionDuration * 1000));
    }

    public async Task PlayLoseShieldAnimation()
    {
        PlayAnimation(loseShieldAnimation.animationName);

        await Task.Delay((int)(loseShieldAnimation.transitionDuration * 1000));

    }

    public async Task PlaySummonAnimation()
    {
        PlayAnimation(summonAnimation.animationName);

        await Task.Delay((int)(summonAnimation.transitionDuration * 1000));
    }

    // Заставить персонажа смотреть в направление клетки
    private void LookAt(Vector3Int target)
    {

        bool direction = Character.tracker.CellToWorld(target).x > transform.parent.position.x;
        if (direction)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
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