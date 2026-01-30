using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    //BAD SOLUTION! REWORK AFTER v0!
    public static void Play()
    {
        SceneManager.LoadScene(1);
    }

    public static void Exit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
        else Application.Quit();
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
