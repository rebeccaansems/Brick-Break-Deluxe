using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public Canvas pauseCanvas;

    public Text highscore1, highscore2, highscore3, highscore4, highscore5;

    private void Start()
    {
        pauseCanvas.enabled = false;
        UpdateHighScores();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0;
            UpdateHighScores();
        }
    }

    public void PauseButtonPressed()
    {
        if (Time.timeScale == 0)
        {
            ReturnButtonPressed();
        }
        else
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0;
            UpdateHighScores();
        }
    }

    public void ReturnButtonPressed()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
    }

    void UpdateHighScores()
    {
        highscore1.text = PlayerPrefs.GetString("PlayerDate1")+ " - " + PlayerPrefs.GetInt("PlayerScore1").ToString("0000");
        highscore2.text = PlayerPrefs.GetString("PlayerDate2")+ " - " + PlayerPrefs.GetInt("PlayerScore2").ToString("0000");
        highscore3.text = PlayerPrefs.GetString("PlayerDate3")+ " - " + PlayerPrefs.GetInt("PlayerScore3").ToString("0000");
        highscore4.text = PlayerPrefs.GetString("PlayerDate4")+ " - " + PlayerPrefs.GetInt("PlayerScore4").ToString("0000");
        highscore5.text = PlayerPrefs.GetString("PlayerDate5")+ " - " + PlayerPrefs.GetInt("PlayerScore5").ToString("0000");
    }
}
