using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class EventEffect
{
    public int healthChange;
    public int moneyChange;
    public int materialChange;
    public int maxHealthChange;
    public int energyChange;
    public int maxEnergyChange;
    public List<GameModule> addModules;
    public List<string> removeModuleIds;   

    /// <summary>
    /// Возвращает строковое описание эффекта с конкретными названиями модулей.
    /// </summary>
    public string GetDescription()
    {
        var sb = new StringBuilder();
        if (healthChange != 0)
            sb.Append(healthChange > 0 ? $"+{healthChange}" : $"{healthChange}").Append(" HP, ");
        if (maxHealthChange != 0)
            sb.Append(maxHealthChange > 0 ? $"+{maxHealthChange}" : $"{maxHealthChange}").Append(" Max HP, ");
        if (energyChange != 0)
            sb.Append(energyChange > 0 ? $"+{energyChange}" : $"{energyChange}").Append(" Energy, ");
        if (maxEnergyChange != 0)
            sb.Append(maxEnergyChange > 0 ? $"+{maxEnergyChange}" : $"{maxEnergyChange}").Append(" Max Energy, ");
        if (moneyChange != 0)
            sb.Append(moneyChange > 0 ? $"+{moneyChange}" : $"{moneyChange}").Append(" Gold, ");
        if (materialChange != 0)
            sb.Append(materialChange > 0 ? $"+{materialChange}" : $"{materialChange}").Append(" Materials, ");

        if (addModules != null && addModules.Count > 0)
            foreach (var mod in addModules)
                sb.Append($"Add {mod.Name}, ");

        if (removeModuleIds != null && removeModuleIds.Count > 0)
            foreach (var modId in removeModuleIds)
                sb.Append($"Remove {modId}, ");

        if (sb.Length == 0)
            return "No effect";
        sb.Length -= 2;
        return sb.ToString();
    }

    /// <summary>
    /// Возвращает true, если в эффекте есть изменения модулей.
    /// </summary>
    public bool HasModuleChanges() =>
        (addModules != null && addModules.Count > 0) ||
        (removeModuleIds != null && removeModuleIds.Count > 0);

    /// <summary>
    /// Формирует текст подсказки с перечнем модулей.
    /// </summary>
    public string GetModuleTooltipText()
    {
        var sb = new StringBuilder();
        if (addModules != null && addModules.Count > 0)
        {
            sb.AppendLine("Добавляет модули:");
            foreach (var mod in addModules)
                sb.AppendLine($"- {mod.Name}");
        }
        if (removeModuleIds != null && removeModuleIds.Count > 0)
        {
            sb.AppendLine("Удаляет модули:");
            foreach (var name in removeModuleIds)
                sb.AppendLine($"- {name}");
        }
        return sb.ToString().TrimEnd();
    }
}