using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int score = 0;
    public int[] brickBreak;
    public float speed, gravityModifier;
    public bool speedBrickEffect = false;
    public bool playerIsDead = false;

    public Text scoreText, highScoreText, unlockBallText, gameOverText1, gameOverText2, gameOverText3, gameOverText4, gameOverText5;
    public ParticleSystem particles;
    public GameObject deathBar;
    public PhysicsMaterial2D noBounce, normalBounce;
    public Canvas gameOverScreen;
    public List<Sprite> allPossibleBalls;
    public List<Color> particleColors;
    public Slider unlockSlider;
    public Image nextBallToUnlockImage;
    public UIButtons uiButtons;

    private int[] unlockedBallThresholds = new int[8] { 500, 1000, 2500, 5000, 10000, 100, 250, 500 };
    private int[] bricksDestroyed;
    private char[] unlockedBallsCharArray;

    private Vector3 gravityOriginal;
    
    void Start()
    {
        playerIsDead = false;
        brickBreak = new int[8];

        uiButtons.OnStart();

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

            PlayerPrefs.SetInt("BricksDestroyed0", 0);
            PlayerPrefs.SetInt("BricksDestroyed1", 0);
            PlayerPrefs.SetInt("BricksDestroyed2", 0);
            PlayerPrefs.SetInt("BricksDestroyed3", 0);
            PlayerPrefs.SetInt("BricksDestroyed4", 0);
            PlayerPrefs.SetInt("BricksDestroyed5", 0);
            PlayerPrefs.SetInt("BricksDestroyed6", 0);
            PlayerPrefs.SetInt("BricksDestroyed7", 0);

            PlayerPrefs.SetFloat("PlayerDeathLevel", 5);
        }
        deathBar.transform.position = new Vector2(0, PlayerPrefs.GetFloat("PlayerDeathLevel"));

        gravityOriginal = new Vector3(0, -12.5f, 0);
        Physics2D.gravity = gravityOriginal;
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");

        Time.timeScale = 1;
        //gameOverScreen.enabled = false;
        UpdateBallColor();
    }

    void OnBecameInvisible()
    {
        PlayerDied();
    }

    void PlayerDied()
    {
        playerIsDead = true;

        uiButtons.OnGameOver();

        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);

        UpdateStats();

        PlayerPrefs.SetFloat("PlayerDeathLevel", Mathf.Min(PlayerPrefs.GetFloat("PlayerDeathLevel"), this.transform.position.y));

        string unlockedBalls = PlayerPrefs.GetString("UnlockedBalls");
        unlockedBallsCharArray = unlockedBalls.ToCharArray();
        bricksDestroyed = new int[8] { PlayerPrefs.GetInt("BricksDestroyed0"), PlayerPrefs.GetInt("BricksDestroyed1"),
            PlayerPrefs.GetInt("BricksDestroyed2"), PlayerPrefs.GetInt("BricksDestroyed3"), PlayerPrefs.GetInt("BricksDestroyed4"),
            PlayerPrefs.GetInt("BricksDestroyed5"), PlayerPrefs.GetInt("BricksDestroyed6"), PlayerPrefs.GetInt("BricksDestroyed7")};

        if (bricksDestroyed[0] > unlockedBallThresholds[0])
        {
            unlockedBallsCharArray[1] = 'U';
        }
        if (bricksDestroyed[1] > unlockedBallThresholds[1])
        {
            unlockedBallsCharArray[2] = 'U';
        }
        if (bricksDestroyed[2] > unlockedBallThresholds[2])
        {
            unlockedBallsCharArray[3] = 'U';
        }
        if (bricksDestroyed[3] > unlockedBallThresholds[3])
        {
            unlockedBallsCharArray[4] = 'U';
        }
        if (bricksDestroyed[4] > unlockedBallThresholds[4])
        {
            unlockedBallsCharArray[5] = 'U';
        }
        if (bricksDestroyed[5] > unlockedBallThresholds[5])
        {
            unlockedBallsCharArray[6] = 'U';
        }
        if (bricksDestroyed[6] > unlockedBallThresholds[6])
        {
            unlockedBallsCharArray[7] = 'U';
        }
        if (bricksDestroyed[7] > unlockedBallThresholds[7])
        {
            unlockedBallsCharArray[8] = 'U';
        }

        unlockedBalls = new string(unlockedBallsCharArray);
        PlayerPrefs.SetString("UnlockedBalls", unlockedBalls);

        if (highScoreText != null)
        {
            highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
        }

        PlayerPrefs.Save();

        Time.timeScale = 0;

        if (gameOverText1 != null)
        {
            gameOverText1.text = PlayerPrefs.GetString("PlayerDate1") + " - " + PlayerPrefs.GetInt("PlayerScore1").ToString("00000000");
            gameOverText2.text = PlayerPrefs.GetString("PlayerDate2") + " - " + PlayerPrefs.GetInt("PlayerScore2").ToString("00000000");
            gameOverText3.text = PlayerPrefs.GetString("PlayerDate3") + " - " + PlayerPrefs.GetInt("PlayerScore3").ToString("00000000");
            gameOverText4.text = PlayerPrefs.GetString("PlayerDate4") + " - " + PlayerPrefs.GetInt("PlayerScore4").ToString("00000000");
            gameOverText5.text = PlayerPrefs.GetString("PlayerDate5") + " - " + PlayerPrefs.GetInt("PlayerScore5").ToString("00000000");
        }

        if (unlockBallText != null)
        {
            int nextBallToUnlock = -1;
            for (int i = 1; i < unlockedBallsCharArray.Length; i++)
            {
                if (unlockedBallsCharArray[i] == 'L')
                {
                    nextBallToUnlock = i - 1;
                    break;
                }
            }

            if (nextBallToUnlock != -1 && unlockedBallThresholds.Length > nextBallToUnlock && bricksDestroyed.Length > nextBallToUnlock)
            {
                unlockBallText.text = bricksDestroyed[nextBallToUnlock] + "/" + unlockedBallThresholds[nextBallToUnlock] + " TO NEXT UNLOCKED BALL";
                unlockSlider.maxValue = unlockedBallThresholds[nextBallToUnlock];
                unlockSlider.value = bricksDestroyed[nextBallToUnlock];
                nextBallToUnlockImage.sprite = allPossibleBalls[nextBallToUnlock+1];
            }
            else
            {
                unlockBallText.text = "ALL BALLS ARE UNLOCKED";
                unlockSlider.maxValue = 50;
                unlockSlider.value = 100;
                nextBallToUnlockImage.enabled = false;
            }
        }
    }

    public void UpdateStats()
    {
        List<KeyValuePair<string, int>> leaderboardScores = new List<KeyValuePair<string, int>>();

        leaderboardScores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("PlayerDate1"), PlayerPrefs.GetInt("PlayerScore1")));
        leaderboardScores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("PlayerDate2"), PlayerPrefs.GetInt("PlayerScore2")));
        leaderboardScores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("PlayerDate3"), PlayerPrefs.GetInt("PlayerScore3")));
        leaderboardScores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("PlayerDate4"), PlayerPrefs.GetInt("PlayerScore4")));
        leaderboardScores.Add(new KeyValuePair<string, int>(PlayerPrefs.GetString("PlayerDate5"), PlayerPrefs.GetInt("PlayerScore5")));

        leaderboardScores.Add(new KeyValuePair<string, int>(DateTime.Today.ToShortDateString(), score));

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

        leaderboardScores.Clear();

        PlayerPrefs.SetInt("BricksDestroyed0", PlayerPrefs.GetInt("BricksDestroyed0") + brickBreak[0]);
        PlayerPrefs.SetInt("BricksDestroyed1", PlayerPrefs.GetInt("BricksDestroyed1") + brickBreak[1]);
        PlayerPrefs.SetInt("BricksDestroyed2", PlayerPrefs.GetInt("BricksDestroyed2") + brickBreak[2]);
        PlayerPrefs.SetInt("BricksDestroyed3", PlayerPrefs.GetInt("BricksDestroyed3") + brickBreak[3]);
        PlayerPrefs.SetInt("BricksDestroyed4", PlayerPrefs.GetInt("BricksDestroyed4") + brickBreak[4]);
        PlayerPrefs.SetInt("BricksDestroyed5", PlayerPrefs.GetInt("BricksDestroyed5") + brickBreak[5]);
        PlayerPrefs.SetInt("BricksDestroyed6", PlayerPrefs.GetInt("BricksDestroyed6") + brickBreak[6]);
        PlayerPrefs.SetInt("BricksDestroyed7", PlayerPrefs.GetInt("BricksDestroyed7") + brickBreak[7]);
    }

    private void FixedUpdate()
    {
        if (score > PlayerPrefs.GetInt("PlayerScore1"))
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

    public void UpdateBallColor()
    {
        this.GetComponent<SpriteRenderer>().sprite = allPossibleBalls[PlayerPrefs.GetInt("CurrentBallSelected")];
        ParticleSystem.MainModule ps = particles.main;
        ps.startColor = particleColors[PlayerPrefs.GetInt("CurrentBallSelected")];
    }

}
