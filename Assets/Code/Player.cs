using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int score = 0;
    public Text scoreText, highScoreText;
    public float speed, gravityModifier;
    public bool speedBrickEffect = false;
    public ParticleSystem particles;
    public GameObject deathBar;
    public PhysicsMaterial2D noBounce, normalBounce;
    public Canvas gameOverScreen;

    private Vector3 gravityOriginal;

    // Use this for initialization
    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerScore1"))
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
        }
        deathBar.transform.position = new Vector2(0, PlayerPrefs.GetFloat("PlayerDeathLevel"));

        gravityOriginal = new Vector3(0, -12.5f, 0);
        Physics2D.gravity = gravityOriginal;
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");

        Time.timeScale = 1;
        gameOverScreen.enabled = false;
    }

    void OnBecameInvisible()
    {
        PlayerDied();
    }

    void PlayerDied()
    {
        if (PlayerPrefs.HasKey("PlayerScore1"))
        {
            List<KeyValuePair<string, int>> leaderboardScores = new List<KeyValuePair<string, int>>();

            leaderboardScores.Add(new KeyValuePair<string,int>(PlayerPrefs.GetString("PlayerDate1"), PlayerPrefs.GetInt("PlayerScore1")));
            leaderboardScores.Add(new KeyValuePair<string,int>(PlayerPrefs.GetString("PlayerDate2"), PlayerPrefs.GetInt("PlayerScore2")));
            leaderboardScores.Add(new KeyValuePair<string,int>(PlayerPrefs.GetString("PlayerDate3"), PlayerPrefs.GetInt("PlayerScore3")));
            leaderboardScores.Add(new KeyValuePair<string,int>(PlayerPrefs.GetString("PlayerDate4"), PlayerPrefs.GetInt("PlayerScore4")));
            leaderboardScores.Add(new KeyValuePair<string,int>(PlayerPrefs.GetString("PlayerDate5"), PlayerPrefs.GetInt("PlayerScore5")));

            leaderboardScores.Add(new KeyValuePair<string,int>(DateTime.Today.ToShortDateString(), score));

            leaderboardScores.Sort((x, y) => x.Value.CompareTo(y.Value));

            PlayerPrefs.SetInt("PlayerScore1", leaderboardScores[5].Value);
            PlayerPrefs.SetInt("PlayerScore2", leaderboardScores[4].Value);
            PlayerPrefs.SetInt("PlayerScore3", leaderboardScores[3].Value);
            PlayerPrefs.SetInt("PlayerScore4", leaderboardScores[2].Value);
            PlayerPrefs.SetInt("PlayerScore5", leaderboardScores[1].Value);

            PlayerPrefs.SetString("PlayerDate1", leaderboardScores[5].Key);
            PlayerPrefs.SetString("PlayerDate2", leaderboardScores[4].Key);
            PlayerPrefs.SetString("PlayerDate3", leaderboardScores[3].Key);
            PlayerPrefs.SetString("PlayerDate4", leaderboardScores[2].Key);
            PlayerPrefs.SetString("PlayerDate5", leaderboardScores[1].Key);

            PlayerPrefs.SetFloat("PlayerDeathLevel", Mathf.Min(PlayerPrefs.GetFloat("PlayerDeathLevel"), this.transform.position.y));

            leaderboardScores.Clear();
        }

        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
        PlayerPrefs.Save();

        Time.timeScale = 0;

        if(gameOverScreen != null)
        {
            gameOverScreen.enabled = true;
            ShowAd();
        }
    }

    void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    private void FixedUpdate()
    {
        if(score > PlayerPrefs.GetInt("PlayerScore1"))
        {
            scoreText.color = Color.yellow;
        }
        scoreText.text = "SCORE: " + score.ToString("00000000");

        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x * speed, -2), ForceMode2D.Force);

        particles.gameObject.transform.position = this.transform.position;
    }

    public void SpeedBrickModeEnabled()
    {
        speedBrickEffect = true;
        if (particles.isPlaying == false)
        {
            particles.Play();
        }
        Physics2D.gravity = Physics2D.gravity * gravityModifier;
        this.GetComponent<Rigidbody2D>().sharedMaterial = noBounce;
        StopCoroutine(SpeedBrickTimer());
        StartCoroutine(SpeedBrickTimer());
    }

    IEnumerator SpeedBrickTimer()
    {
        yield return new WaitForSeconds(15);
        speedBrickEffect = false;
        Physics2D.gravity = gravityOriginal;
        this.GetComponent<Rigidbody2D>().sharedMaterial = normalBounce;
        particles.Stop();
    }

}
