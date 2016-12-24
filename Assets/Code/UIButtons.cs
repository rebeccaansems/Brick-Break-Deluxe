using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIButtons : MonoBehaviour
{
    public Canvas pauseCanvas, optionsCanvas;
    public Text highscoreOverlay;
    public Text highscore1, highscore2, highscore3, highscore4, highscore5;

    public GameObject deathBar;

    private void Start()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            UpdateHighScores();
        }
        optionsCanvas.enabled = false;
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
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
        }
        optionsCanvas.enabled = false;
        Time.timeScale = 1;
    }

    void UpdateHighScores()
    {
        if (highscore1 != null)
        {
            highscore1.text = PlayerPrefs.GetString("PlayerDate1") + " - " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
            highscore2.text = PlayerPrefs.GetString("PlayerDate2") + " - " + PlayerPrefs.GetInt("PlayerScore2").ToString("00000000");
            highscore3.text = PlayerPrefs.GetString("PlayerDate3") + " - " + PlayerPrefs.GetInt("PlayerScore3").ToString("00000000");
            highscore4.text = PlayerPrefs.GetString("PlayerDate4") + " - " + PlayerPrefs.GetInt("PlayerScore4").ToString("00000000");
            highscore5.text = PlayerPrefs.GetString("PlayerDate5") + " - " + PlayerPrefs.GetInt("PlayerScore5").ToString("00000000");

            highscoreOverlay.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptionsButtonPressed()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
        }
        optionsCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void ResetScoreButtonPressed()
    {
        PlayerPrefs.SetInt("PlayerScore1", 0);
        PlayerPrefs.SetInt("PlayerScore2", 0);
        PlayerPrefs.SetInt("PlayerScore3", 0);
        PlayerPrefs.SetInt("PlayerScore4", 0);
        PlayerPrefs.SetInt("PlayerScore5", 0);

        PlayerPrefs.SetString("PlayerDate1", DateTime.Today.ToShortDateString());
        PlayerPrefs.SetString("PlayerDate2", DateTime.Today.ToShortDateString());
        PlayerPrefs.SetString("PlayerDate3", DateTime.Today.ToShortDateString());
        PlayerPrefs.SetString("PlayerDate4", DateTime.Today.ToShortDateString());
        PlayerPrefs.SetString("PlayerDate5", DateTime.Today.ToShortDateString());

        PlayerPrefs.SetFloat("PlayerDeathLevel", 5);

        if(deathBar != null)
        {
            deathBar.transform.position = new Vector2(0, PlayerPrefs.GetFloat("PlayerDeathLevel"));
        }

        UpdateHighScores();
        ReturnButtonPressed();
    }
}
