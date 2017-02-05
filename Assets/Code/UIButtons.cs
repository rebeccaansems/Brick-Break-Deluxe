﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIButtons : MonoBehaviour
{
    public static float sfxVolume, musicVolume;

    public AudioClip buttonPress;

    public Canvas pauseCanvas, optionsCanvas, gameoverCanvas, gameStatsCanvas;

    public Button pauseButton, unpausePanel, returnButton, gameStatsButton;

    public Slider sfxSlider, musicSlider;

    public Text highscoreOverlay;
    public Text highscore1, highscore2, highscore3, highscore4, highscore5;
    public Text GOhighscore1, GOhighscore2, GOhighscore3, GOhighscore4, GOhighscore5;
    public Text[] stats;

    public Image selectedBall, lockedIcon;

    public List<Sprite> ballColors;

    public GameObject deathBar, player;

    private int currentBall;
    private bool canChangeVolume = false;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("GamesPlayed"))
        {
            PlayerPrefs.SetInt("GamesPlayed", 0);
            PlayerPrefs.SetInt("CurrentBallSelected", 0);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetString("UnlockedBalls", "ULLLLL");
        }

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

            canChangeVolume = true;
            AdjustVolume();
        }


        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            UpdateHighScores();
        }

        if (optionsCanvas != null)
        {
            optionsCanvas.enabled = false;
            currentBall = PlayerPrefs.GetInt("CurrentBallSelected");
            selectedBall.sprite = ballColors[currentBall];
        }

        if (gameStatsCanvas != null)
        {
            gameStatsCanvas.enabled = false;
        }

        unpausePanel.enabled = false;
        unpausePanel.GetComponent<Image>().raycastTarget = false;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            pauseButton.enabled = true;
            pauseButton.enabled = true;
            pauseCanvas.enabled = true;
            Time.timeScale = 0;
            UpdateHighScores();
        }
    }

    public void PauseButtonPressed()
    {
        if (!gameoverCanvas.enabled)
        {
            pauseButton.GetComponent<Image>().color = Color.grey;
            unpausePanel.enabled = true;
            unpausePanel.GetComponent<Image>().raycastTarget = true;

            if (Time.timeScale == 0)
            {
                ReturnButtonPressed();
            }
            else
            {
                UpdateHighScores();
                pauseCanvas.enabled = true;
                Time.timeScale = 0;
            }
        }
    }

    public void ReturnButtonPressed()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
        }

        if (optionsCanvas != null)
        {
            optionsCanvas.enabled = false;
        }

        if (pauseButton != null)
        {
            pauseButton.GetComponent<Image>().color = Color.white;
        }

        if (gameStatsCanvas != null)
        {
            gameStatsCanvas.enabled = false;
        }

        unpausePanel.enabled = false;
        unpausePanel.GetComponent<Image>().raycastTarget = false;

        Time.timeScale = 1;
    }

    void UpdateHighScores()
    {
        if (player != null)
        {
            player.GetComponent<Player>().UpdateStats();
            player.GetComponent<Player>().brickBreak = new int[8];
        }

        if (highscore1 != null)
        {
            highscore1.text = PlayerPrefs.GetString("PlayerDate1") + " - " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
            highscore2.text = PlayerPrefs.GetString("PlayerDate2") + " - " + PlayerPrefs.GetInt("PlayerScore2").ToString("00000000");
            highscore3.text = PlayerPrefs.GetString("PlayerDate3") + " - " + PlayerPrefs.GetInt("PlayerScore3").ToString("00000000");
            highscore4.text = PlayerPrefs.GetString("PlayerDate4") + " - " + PlayerPrefs.GetInt("PlayerScore4").ToString("00000000");
            highscore5.text = PlayerPrefs.GetString("PlayerDate5") + " - " + PlayerPrefs.GetInt("PlayerScore5").ToString("00000000");

            highscoreOverlay.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
        }

        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnOptionsButtonPressed()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
        }

        unpausePanel.enabled = true;
        unpausePanel.GetComponent<Image>().raycastTarget = true;

        optionsCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void ResetScoreButtonPressed()
    {
        PlayerPrefs.SetInt("GamesPlayed", 0);

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

        PlayerPrefs.SetInt("BricksDestroyed0", 0);
        PlayerPrefs.SetInt("BricksDestroyed1", 0);
        PlayerPrefs.SetInt("BricksDestroyed2", 0);
        PlayerPrefs.SetInt("BricksDestroyed3", 0);
        PlayerPrefs.SetInt("BricksDestroyed4", 0);
        PlayerPrefs.SetInt("BricksDestroyed5", 0);
        PlayerPrefs.SetInt("BricksDestroyed6", 0);
        PlayerPrefs.SetInt("BricksDestroyed7", 0);

        PlayerPrefs.SetInt("CurrentBallSelected", 0);
        PlayerPrefs.SetString("UnlockedBalls", "ULLLLL");

        PlayerPrefs.SetFloat("PlayerDeathLevel", 5);

        if (deathBar != null)
        {
            deathBar.transform.position = new Vector2(0, PlayerPrefs.GetFloat("PlayerDeathLevel"));
        }

        UpdateHighScores();
        ReturnButtonPressed();
    }

    public void BallDirectionButtonPressed(int direction)
    {
        currentBall = currentBall + direction;

        if (currentBall > ballColors.Count - 1)
        {
            currentBall = 0;
        }
        else if (currentBall < 0)
        {
            currentBall = ballColors.Count - 1;
        }

        string unlockedBalls = PlayerPrefs.GetString("UnlockedBalls");
        bool isLocked = false;

        if (unlockedBalls.ToCharArray()[currentBall] == 'L')
        {
            lockedIcon.enabled = true;
            isLocked = true;
            returnButton.interactable = false;
            gameStatsButton.interactable = false;
            unpausePanel.interactable = false;

            if (pauseButton != null)
            {
                pauseButton.interactable = false;
            }
        }
        else
        {
            lockedIcon.enabled = false;
            returnButton.interactable = true;
            gameStatsButton.interactable = true;
            unpausePanel.interactable = true;

            if (pauseButton != null)
            {
                pauseButton.interactable = true;
            }
        }

        selectedBall.sprite = ballColors[currentBall];
        if (!isLocked)
        {
            PlayerPrefs.SetInt("CurrentBallSelected", currentBall);

            if (player != null)
            {
                player.GetComponent<Player>().UpdateBallColor();
            }
        }
    }

    public void GameStatsButtonPressed()
    {
        gameStatsCanvas.enabled = true;

        if (player != null)
        {
            player.GetComponent<Player>().UpdateStats();
        }

        stats[0].text = PlayerPrefs.GetInt("BricksDestroyed0").ToString();
        stats[1].text = PlayerPrefs.GetInt("BricksDestroyed1").ToString();
        stats[2].text = PlayerPrefs.GetInt("BricksDestroyed2").ToString();
        stats[3].text = PlayerPrefs.GetInt("BricksDestroyed3").ToString();
        stats[4].text = PlayerPrefs.GetInt("BricksDestroyed4").ToString();
        stats[5].text = PlayerPrefs.GetInt("BricksDestroyed5").ToString();
        stats[6].text = PlayerPrefs.GetInt("BricksDestroyed6").ToString();
        stats[7].text = PlayerPrefs.GetInt("BricksDestroyed7").ToString();
        stats[8].text = PlayerPrefs.GetInt("GamesPlayed").ToString();
    }

    public void PlayButtonNoise()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonPress, sfxVolume);
    }

    public void AdjustVolume()
    {
        if (canChangeVolume)
        {
            sfxVolume = sfxSlider.value;
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

            musicVolume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }
    }
}
