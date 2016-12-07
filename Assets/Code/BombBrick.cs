using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBrick : MonoBehaviour
{
    public ParticleSystem particles;
    public int color;

    private bool collidedWithPlayer = false, wasVisible = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collidedWithPlayer == false)
        {
            collidedWithPlayer = true;
            DestroySurroundingBricks();
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

    public void DestroySurroundingBricks()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 2);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Brick")
            {
                if (hitColliders[i].gameObject.GetComponent<Bricks>() != null)
                {
                    hitColliders[i].gameObject.GetComponent<Bricks>().DestroyBrick(true);
                }
                else if (hitColliders[i].gameObject.GetComponent<BombBrick>() != null)
                {
                    hitColliders[i].gameObject.GetComponent<BombBrick>().DestroyBrick(true);
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
        yield return new WaitForSeconds(0.05f);
        particles.Play();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
        yield break;
    }
}

