using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int score = 0;
    public Text scoreText, highScoreText;
    public float speed, gravityModifier;
    public bool speedBrickEffect = false;
    public ParticleSystem particles;
    public GameObject[] deathBars;

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

            PlayerPrefs.SetInt("PlayerDeathLevel1", 5);
            PlayerPrefs.SetInt("PlayerDeathLevel2", 5);
            PlayerPrefs.SetInt("PlayerDeathLevel3", 5);
            PlayerPrefs.SetInt("PlayerDeathLevel4", 5);
            PlayerPrefs.SetInt("PlayerDeathLevel5", 5);
        }

        SetDeathMarkers();

        gravityOriginal = new Vector3(0, -9.81f, 0);
        Physics2D.gravity = gravityOriginal;
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x > 3.6 || this.transform.position.x < -3.6)
        {
            PlayerDied();
        }
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

            List<int> deathLevels = new List<int>();

            deathLevels.Add(PlayerPrefs.GetInt("PlayerDeathLevel1"));
            deathLevels.Add(PlayerPrefs.GetInt("PlayerDeathLevel2"));
            deathLevels.Add(PlayerPrefs.GetInt("PlayerDeathLevel3"));
            deathLevels.Add(PlayerPrefs.GetInt("PlayerDeathLevel4"));
            deathLevels.Add(PlayerPrefs.GetInt("PlayerDeathLevel5"));
            deathLevels.Add((int)this.transform.position.y);

            deathLevels.Sort((x, y) => x.CompareTo(y));

            PlayerPrefs.SetInt("PlayerDeathLevel1", deathLevels[0]);
            PlayerPrefs.SetInt("PlayerDeathLevel2", deathLevels[1]);
            PlayerPrefs.SetInt("PlayerDeathLevel3", deathLevels[2]);
            PlayerPrefs.SetInt("PlayerDeathLevel4", deathLevels[3]);
            PlayerPrefs.SetInt("PlayerDeathLevel5", deathLevels[4]);

            deathLevels.Clear();
            leaderboardScores.Clear();
        }

        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetDeathMarkers();
    }

    private void SetDeathMarkers()
    {
        deathBars[0].transform.position = new Vector2(0, PlayerPrefs.GetInt("PlayerDeathLevel1"));
        deathBars[1].transform.position = new Vector2(0, PlayerPrefs.GetInt("PlayerDeathLevel2"));
        deathBars[2].transform.position = new Vector2(0, PlayerPrefs.GetInt("PlayerDeathLevel3"));
        deathBars[3].transform.position = new Vector2(0, PlayerPrefs.GetInt("PlayerDeathLevel4"));
        deathBars[4].transform.position = new Vector2(0, PlayerPrefs.GetInt("PlayerDeathLevel5"));
    }

    private void FixedUpdate()
    {
        if(score > PlayerPrefs.GetInt("PlayerScore1"))
        {
            scoreText.color = Color.yellow;
        }
        scoreText.text = "SCORE: " + score.ToString("00000000");

        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x * speed, 0));
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
        StopCoroutine(SpeedBrickTimer());
        StartCoroutine(SpeedBrickTimer());
    }

    IEnumerator SpeedBrickTimer()
    {
        yield return new WaitForSeconds(15);
        speedBrickEffect = false;
        Physics2D.gravity = gravityOriginal;
        particles.Stop();
    }

}
