using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGem : MonoBehaviour
{
    public Text score;
    public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + (3+(int)this.transform.position.y*(-1));
        if(this.transform.position.x > 3.6 || this.transform.position.x < -3.6)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x * speed,0));
    }

}
