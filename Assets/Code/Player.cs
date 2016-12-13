using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public float speed, gravityModifier;
    public bool speedBrickEffect = false;
    public ParticleSystem particles;

    private Vector3 gravityOriginal;

    // Use this for initialization
    void Start()
    {
        gravityOriginal = new Vector3(0, -9.81f, 0);
        Physics2D.gravity = gravityOriginal;
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
            List<int> leaderboardScores = new List<int>();

            leaderboardScores.Add(PlayerPrefs.GetInt("PlayerScore1"));
            leaderboardScores.Add(PlayerPrefs.GetInt("PlayerScore2"));
            leaderboardScores.Add(PlayerPrefs.GetInt("PlayerScore3"));
            leaderboardScores.Add(PlayerPrefs.GetInt("PlayerScore4"));
            leaderboardScores.Add(PlayerPrefs.GetInt("PlayerScore5"));

            leaderboardScores.Add(score);

            leaderboardScores.Sort();

            PlayerPrefs.SetInt("PlayerScore1", leaderboardScores[5]);
            PlayerPrefs.SetInt("PlayerScore2", leaderboardScores[4]);
            PlayerPrefs.SetInt("PlayerScore3", leaderboardScores[3]);
            PlayerPrefs.SetInt("PlayerScore4", leaderboardScores[2]);
            PlayerPrefs.SetInt("PlayerScore5", leaderboardScores[1]);

            Debug.Log(PlayerPrefs.GetInt("PlayerScore1") + " " + PlayerPrefs.GetInt("PlayerScore2") + " " + PlayerPrefs.GetInt("PlayerScore3") + " " + PlayerPrefs.GetInt("PlayerScore4") + " " + PlayerPrefs.GetInt("PlayerScore5"));

        }
        else
        {
            PlayerPrefs.SetInt("PlayerScore1", 0);
            PlayerPrefs.SetInt("PlayerScore2", 0);
            PlayerPrefs.SetInt("PlayerScore3", 0);
            PlayerPrefs.SetInt("PlayerScore4", 0);
            PlayerPrefs.SetInt("PlayerScore5", 0);

            PlayerDied();
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FixedUpdate()
    {
        scoreText.text = "Score: " + score;

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
