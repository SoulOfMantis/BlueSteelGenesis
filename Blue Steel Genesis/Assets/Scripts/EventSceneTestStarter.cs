using UnityEngine;

public class EventSceneTestStarter : MonoBehaviour
{
    public EventData testEvent; 

    void Start()
    {
        
        var controller = FindObjectOfType<EventSceneController>();
        if (controller != null && testEvent != null)
        {
            
            controller.SetEvent(testEvent);
        }
    }
}