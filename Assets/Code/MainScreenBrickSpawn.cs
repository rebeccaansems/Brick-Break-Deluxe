using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenBrickSpawn : MonoBehaviour
{

    public Sprite[] brickColorsRect;
    public Material[] brickColorsMat;
    public GameObject brick;

    // Use this for initialization
    void Start()
    {
        MainScreenBricks[] newBrick = this.gameObject.GetComponentsInChildren<MainScreenBricks>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            int brickColorChosen = Random.Range(0, 5);

            newBrick[i].GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
            newBrick[i].GetComponent<SpriteRenderer>().sprite = brickColorsRect[brickColorChosen];

        }
    }

    public void createNewBrick(Vector2 location)
    {
        GameObject newBrick = Instantiate(brick) as GameObject;
        newBrick.transform.position = location;
        int brickColorChosen = Random.Range(0, 5);

        newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
        newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRect[brickColorChosen];
    }
}
