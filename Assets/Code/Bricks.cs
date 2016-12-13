using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public ParticleSystem particles;
    public int color, brickType;

    private Player player;
    private bool collidedWithPlayer = false, wasVisible = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collidedWithPlayer == false)
        {
            if (brickType == 2)//Speed brick
            {
                collision.gameObject.GetComponent<Player>().SpeedBrickModeEnabled();
            }
            else if (brickType == 3)//Slowmo brick
            {

            }

            collidedWithPlayer = true;
            CheckBricksAround();
            DestroyBrick(true);
        }
    }

    private void OnBecameVisible()
    {
        wasVisible = true;
    }

    private void OnBecameInvisible()
    {
        //destroy brick because it went off screen but is not currently doing the DestroyBrick IEnumerator
        if (collidedWithPlayer == false && wasVisible == true && particles.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
    }

    public void CheckBricksAround()
    {
        int radius = 1;
        bool colorMatch = true;

        if (brickType == 1)
        {
            radius = 2;
            colorMatch = false;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Brick")
            {
                if (hitColliders[i].GetComponent<Bricks>().color == this.color || colorMatch == false)
                {
                    hitColliders[i].gameObject.GetComponent<Bricks>().DestroyBrick(true);
                    player.score++;
                }
            }
            i++;
        }
    }

    public void DestroyBrick(bool starterTimer)
    {
        StartCoroutine(DestroyBrickTimer(starterTimer));
    }

    IEnumerator DestroyBrickTimer(bool starterTimer)
    {
        if (player.speedBrickEffect == false)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
        particles.Play();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
        yield break;
    }
}
