using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    public GameObject brick, wind;
    public Sprite[] brickColorsRect, brickColorsSquare;
    public Material[] brickColorsMat;

    private float currentX = -1.8f, currentY = 2;

    private void Start()
    {
        SpawnBrick();
    }

    public void SpawnBrick()
    {
        int brickCounter = 0, rowNumber = 1;
        int brickType = Random.Range(0, 30);

        for (int i = 1; i < 60; i++)
        {

            if (brickType < 29)//normal brick
            {
                currentX += 1.2f;
                if (brickCounter % 4 == 0 && rowNumber % 2 == 0)
                {
                    currentY -= 0.7f;
                    currentX = -1.2f;
                    brickCounter = 0;
                    brickType = Random.Range(0, 20);
                    rowNumber++;
                }
                else if (brickCounter % 3 == 0 && rowNumber % 2 != 0)
                {
                    currentY -= 0.7f;
                    currentX = -1.8f;
                    brickCounter = 0;
                    brickType = Random.Range(0, 20);
                    rowNumber++;
                }

                if (Random.Range(0, 10) < 9)
                {
                    int brickColorChosen = Random.Range(0, 5);
                    GameObject newBrick = Instantiate(brick);
                    newBrick.transform.position = new Vector2(currentX, currentY);
                    newBrick.GetComponent<Bricks>().color = brickColorChosen;
                    newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRect[brickColorChosen];
                    newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                    newBrick.transform.parent = this.transform;
                }
                brickCounter++;
            }
            else//fan
            {
                currentY -= 0.7f;
                int brickColorChosen = Random.Range(0, 5);

                GameObject newWind = Instantiate(wind);
                newWind.transform.position = new Vector2(2.85f, currentY);
                newWind.GetComponent<SpriteRenderer>().sprite = brickColorsSquare[brickColorChosen];
                newWind.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                newWind.transform.parent = this.transform;
                currentY -= 0.7f;

                brickType = Random.Range(0, 20);
            }
        }
    }
}
