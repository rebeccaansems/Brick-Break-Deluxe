using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    public GameObject brick, wind;
    public Sprite[] brickColorsRect, brickColorsRectSpecial1, brickColorsRectSpecial2, brickColorsRectSpecial3, brickColorsSquare;
    public Material[] brickColorsMat;

    private float currentX = -1.8f, currentY = -2, yChange = -0.7f, brickScaleX = 1f, brickScaleY = 1f;

    private float xModifier, yModifier;

    private void Start()
    {
        xModifier = Screen.width / 333f;
        yModifier = Screen.height / 534f;

        yChange = -yChange * yModifier;
        currentX = currentX * xModifier;
        currentY = currentY * yModifier;

        brickScaleX = (Screen.width / 57f) / (333 / 57f);
        brickScaleY = (Screen.height / 29f) / (534 / 29f);

        this.transform.localScale = new Vector2(brickScaleX, brickScaleY);

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
                currentX += 1.2f * xModifier;
                if (brickCounter % 4 == 0 && rowNumber % 2 == 0)
                {
                    currentY -= yChange;
                    currentX = -2.4f * xModifier;
                    brickCounter = 0;
                    brickOrOther = Random.Range(0, 20);
                    rowNumber++;
                }
                else if (brickCounter % 5 == 0 && rowNumber % 2 != 0)
                {
                    currentY -= yChange;
                    currentX = -1.8f * xModifier;
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
                    newBrick.GetComponent<Bricks>().brickBase = brickColorsRect[brickColorChosen];
                    newBrick.GetComponent<Bricks>().brickBaseParticles = brickColorsMat[brickColorChosen];
                    newBrick.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                    newBrick.transform.position = new Vector2(currentX, currentY);
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
                    else if (brickType < 50)//Color Change Bricks ~ 0.3% chance
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
                currentY -= yChange;
                int brickColorChosen = Random.Range(0, 5);

                GameObject newWind = Instantiate(wind);
                newWind.transform.position = new Vector2(4f * xModifier, currentY);
                newWind.transform.localScale = new Vector2(brickScaleX, brickScaleY);
                newWind.GetComponent<SpriteRenderer>().sprite = brickColorsSquare[brickColorChosen];
                newWind.GetComponentInChildren<ParticleSystemRenderer>().material = brickColorsMat[brickColorChosen];
                newWind.transform.parent = this.transform;

                brickOrOther = Random.Range(0, 20);
            }
        }
    }
}
