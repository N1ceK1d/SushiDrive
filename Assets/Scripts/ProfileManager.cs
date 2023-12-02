using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public TMP_Text fullName;
    
    private void Awake() {
        if(PlayerPrefs.GetInt("user_id") != null)
        {
            fullName.text = PlayerPrefs.GetString("FirstName") + " " + PlayerPrefs.GetString("SecondName");
        }    
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("New Scene");
    }
}
