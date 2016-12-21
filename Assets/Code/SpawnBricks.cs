using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    public GameObject brick, wind;
    public Sprite[] brickColorsRect, brickColorsRectSpecial1, brickColorsRectSpecial2, brickColorsRectSpecial3, brickColorsSquare;
    public Material[] brickColorsMat;

    private int rowNumber = 1;
    private float xChange, yChange, xScale, yScale, xCurrent, yCurrent;

    private void Start()
    {
        xChange = (Screen.width / (57 * 4.5f));
        yChange = (Screen.height / (29 * 20f));

        xScale = Screen.width / (57 * 4.5f);
        yScale = Screen.height / (29 * 15f);

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
        int brickCounter = 0;
        int brickChance = Mathf.Max(1000 - (rowNumber / 10), 500);
        int blankBrickChance = Mathf.Max(100 - (rowNumber / 100), 75);
        int brickOrOther = Random.Range(0, 1000);

        for (int i = 1; i < 64; i++)
        {
            if (brickOrOther < brickChance)//brick
            {
                xCurrent += 1.2f * xChange;
                if (brickCounter % 4 == 0 && rowNumber % 2 == 0)
                {
                    yCurrent -= yChange;
                    xCurrent = -2.4f * xChange;
                    brickCounter = 0;
                    brickOrOther = Random.Range(0, 20);
                    rowNumber++;
                }
                else if (brickCounter % 5 == 0 && rowNumber % 2 != 0)
                {
                    yCurrent -= yChange;
                    xCurrent = -1.8f * xChange;
                    brickCounter = 0;
                    brickOrOther = Random.Range(0, 20);
                    rowNumber++;
                }

                if (Random.Range(0, 100) < blankBrickChance)
                {
                    int brickColorChosen = Random.Range(0, 5);
                    int brickType = Random.Range(0, 1000);

                    GameObject newBrick = Instantiate(brick);

                    newBrick.GetComponent<Bricks>().color = brickColorChosen;
                    newBrick.GetComponent<Bricks>().brickBase = brickColorsRect[brickColorChosen];
                    newBrick.GetComponent<Bricks>().brickBaseParticles = brickColorsMat[brickColorChosen];
                    newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                    newBrick.transform.position = new Vector2(xCurrent, yCurrent);
                    newBrick.transform.localScale = new Vector2(xScale, yScale);
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
                    else if (brickType < 15)//Color Change Bricks ~ 0.3% chance
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
                yCurrent -= yChange;
                int brickColorChosen = Random.Range(0, 5);

                GameObject newWind = Instantiate(wind);
                newWind.transform.position = new Vector2(4f * xChange, yCurrent);
                newWind.transform.localScale = new Vector2(xScale, yScale);
                newWind.GetComponent<SpriteRenderer>().sprite = brickColorsSquare[brickColorChosen];
                newWind.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                newWind.transform.parent = this.transform;
                rowNumber++;

                brickOrOther = Random.Range(0, 20);
            }
        }
    }
}
