using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{

    public GameObject brick;
    public Sprite[] brickColors;
    public Material[] brickColorsMat;

    private float currentX = -1.8f, currentY = 2;

    // Use this for initialization
    void Start()
    {
        SpawnBrick();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBrick()
    {
        int brickCounter = 0, rowNumber = 1;

        for (int i = 1; i < 60; i++)
        {
            currentX += 1.2f;
            if (brickCounter % 4 == 0 && rowNumber % 2 == 0)
            {
                currentY -= 0.7f;
                currentX = -1.2f;
                brickCounter = 0;
                rowNumber++;
            }
            else if (brickCounter % 3 == 0 && rowNumber % 2 != 0)
            {
                currentY -= 0.7f;
                currentX = -1.8f;
                brickCounter = 0;
                rowNumber++;
            }

            if (Random.Range(0, 10) < 9)
            {
                int brickColorChosen = Random.Range(0, 5);
                GameObject newBrick = Instantiate(brick);
                newBrick.transform.position = new Vector3(currentX, currentY);
                newBrick.GetComponent<SpriteRenderer>().sprite = brickColors[brickColorChosen];
                newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                newBrick.transform.parent = this.transform;
            }

            brickCounter++;
        }
    }
}
