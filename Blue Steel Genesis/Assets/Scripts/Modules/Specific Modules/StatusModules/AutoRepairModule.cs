using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��������� ������ ���  - ������� ���� ��� ������ ����
/// </summary>
public class AutoRepairModule : PositiveStatusModule
{
    protected uint healthGained;
    public AutoRepairModule() : base()
    {
        triggerType = TriggerType.OnTurnEnd;
        healthGained = 2;
        turnsLeft.Value = 1;
        //Icon_name = "AutoRepairModule";
    }
    public AutoRepairModule(uint health, uint duration) :this()
    {
        healthGained = health;
        turnsLeft.Value = duration;
    }
    public override string Description()
    {
        return $"Restores {healthGained} health to the target at the end of the next {turnsLeft} turns." + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos, ActionContext ctx)
    {
        await user.heal(healthGained, MakeContext(user, pos));
        Debug.Log($"AutoRepair restored {healthGained} health to {user.GetType().Name}");
        turnTick();
    }

    public override void Refresh(StatusModule other)
    {
        if (other is AutoRepairModule a)
        {
            healthGained = Math.Max(healthGained, a.healthGained);
            turnsLeft += a.turnsLeft;
            UpdateTooltipIfCurrent();
        }
    }

    public override bool IsExpired()
    {
        return turnsLeft <= 0;
    }
}