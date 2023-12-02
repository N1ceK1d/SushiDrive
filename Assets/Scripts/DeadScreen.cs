using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DeadScreen : MonoBehaviour
{
    public GameObject endScreen;
    public int totalScore;
    public TMP_Text endScore;
    public GameObject scoreData;

    private void Awake() {
        endScreen.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToProfile()
    {
        if(PlayerPrefs.HasKey("user_id"))
        {
            WWWForm regForm = new WWWForm();
            regForm.AddField("user_id", PlayerPrefs.GetInt("user_id"));
		    regForm.AddField("score", totalScore);

		    WWW www = new WWW("http://localhost/SushiDriver/php/updateScore.php", regForm);
		    StartCoroutine(UpdateFunc(www));
            Time.timeScale = 1f;
            SceneManager.LoadScene("Profile");
        }
        else 
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator UpdateFunc(WWW www)
	{
		yield return www;
        Debug.Log(www.text);
		if(www.error != null)
		{
			Debug.Log("Ошибка: " + www.error);
			yield break;
		}
	}

    public void ShowDeadScreen()
    {
        endScreen.SetActive(true);
        scoreData.SetActive(false);
        Time.timeScale = 0f;
    }
}
