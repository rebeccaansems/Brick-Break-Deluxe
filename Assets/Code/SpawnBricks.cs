using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    public GameObject brick, wind;
    public Sprite[] brickColorsRect, brickColorsRectSpecial1, brickColorsRectSpecial2, brickColorsRectSpecial3, brickColorsSquare;
    public Material[] brickColorsMat;

    private float currentX = -1.8f, currentY = 2;

    private void Start()
    {
        SpawnBrick();
        StartCoroutine(BrickSpawnTimer());
    }

    IEnumerator BrickSpawnTimer()
    {
        while (true)
        {
            SpawnBrick();
            yield return new WaitForSeconds(4);
        }
    }

    public void SpawnBrick()
    {
        int brickCounter = 0, rowNumber = 1;
        int brickOrOther = Random.Range(0, 100);

        for (int i = 1; i < 60; i++)
        {
            if (brickOrOther < 70)//normal brick
            {
                currentX += 1.2f;
                if (brickCounter % 4 == 0 && rowNumber % 2 == 0)
                {
                    currentY -= 0.7f;
                    currentX = -2.4f;
                    brickCounter = 0;
                    brickOrOther = Random.Range(0, 20);
                    rowNumber++;
                }
                else if (brickCounter % 5 == 0 && rowNumber % 2 != 0)
                {
                    currentY -= 0.7f;
                    currentX = -1.8f;
                    brickCounter = 0;
                    brickOrOther = Random.Range(0, 20);
                    rowNumber++;
                }

                if (Random.Range(0, 10) < 9)
                {
                    int brickColorChosen = Random.Range(0, 5);
                    int brickType = Random.Range(0, 1000);

                    GameObject newBrick = Instantiate(brick);

                    newBrick.GetComponent<Bricks>().color = brickColorChosen;
                    newBrick.transform.position = new Vector2(currentX, currentY);
                    newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                    newBrick.transform.parent = this.transform;

                    if (brickType < 10)//Bomb Bricks ~ 1% chance
                    {
                        newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRectSpecial1[brickColorChosen];
                        newBrick.GetComponent<Bricks>().brickType = 1;
                    }
                    else if (brickType < 12)//Speed Brick ~ 0.2% chance
                    {
                        newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRectSpecial2[brickColorChosen];
                        newBrick.GetComponent<Bricks>().brickType = 2;
                    }
                    else if (brickType < 15)//Slow Mo Bricks ~ 0.3% chance
                    {
                        newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRectSpecial3[brickColorChosen];
                        newBrick.GetComponent<Bricks>().brickType = 3;
                    }
                    else
                    {
                        newBrick.GetComponent<SpriteRenderer>().sprite = brickColorsRect[brickColorChosen];
                        newBrick.GetComponent<Bricks>().brickType = 0;
                    }
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
                //currentY -= 0.7f;

                brickOrOther = Random.Range(0, 20);
            }
        }
    }
}
