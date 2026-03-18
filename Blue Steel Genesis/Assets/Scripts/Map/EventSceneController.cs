using UnityEngine;
using UnityEngine.UI;

public class EventSceneController : MonoBehaviour
{
    public Text eventNameText;
    public Button exitButton;
    public Button battleButton;
    public Button shopButton;

    private EventData eventData;

    void Start()
    {

        if (GameState.CurrentExpedition != null)
        {

        }

        exitButton.onClick.AddListener(OnExit);
        battleButton.onClick.AddListener(OnBattle);
        shopButton.onClick.AddListener(OnShop);
    }

    void OnExit()
    {
        GameState.CurrentExpedition?.HandleEventOutcome(EventOutcome.Exit);
    }

    void OnBattle()
    {
        GameState.CurrentExpedition?.HandleEventOutcome(EventOutcome.EnterBattle);
    }

    void OnShop()
    {
        GameState.CurrentExpedition?.HandleEventOutcome(EventOutcome.EnterShop);
    }
}