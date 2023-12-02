using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame_Btn()
    {
        if(PlayerPrefs.HasKey("user_id"))
        {
            SceneManager.LoadScene("Profile");
        }
        else 
        {
            SceneManager.LoadScene("New Scene");
        }
    }

    public void StartGuest()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
