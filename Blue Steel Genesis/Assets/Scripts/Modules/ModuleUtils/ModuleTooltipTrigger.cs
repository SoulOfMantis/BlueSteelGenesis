using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Компонент для отображения всплывающей подсказки модуля при наведении.
/// Используется как на кнопках ModuleEntry, так и на кнопках выбора в событиях.
/// </summary>
public class ModuleTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameModule currentModule;

    /// <summary>
    /// Обновляет модуль, информацию о котором нужно показывать.
    /// Вызывается извне перед активацией триггера.
    /// </summary>
    public void updateModuleTrigger(GameModule module)
    {
        currentModule = module;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentModule == null)
            return;

        // Предполагается, что TooltipSystem уже существует в проекте и имеет сигнатуру:
        // public static void ShowTooltip(TooltipType type, string title, string description, string iconName)
        TooltipSystem.ShowTooltip(
            TooltipSystem.TooltipType.moduleTooltip,
            currentModule.Name,
            currentModule.Description(),
            currentModule.Icon_name
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.HideTooltip(TooltipSystem.TooltipType.moduleTooltip);
    }
}