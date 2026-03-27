using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ModuleManagementUIToggleButton : MonoBehaviour
{
    [SerializeField] ModuleManagementUI UI;
    Button toggleButton;
    void ToggleUI() => UI.gameObject.SetActive(!UI.gameObject.activeSelf);
    private void Start()
    {
        toggleButton = GetComponent<Button>();
        toggleButton.onClick.AddListener(ToggleUI);
    }
}

