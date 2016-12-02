using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGem : MonoBehaviour
{
    public Text score;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + (3+(int)this.transform.position.y*(-1));
    }

}
