using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collidedWithPlayer == false)
        {
            collidedWithPlayer = true;
            CheckBricksAround();
            DestroyBrick();
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 1);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Brick")
            {
                if (hitColliders[i].GetComponent<Bricks>().color == this.color)
                {
                    hitColliders[i].gameObject.GetComponent<Bricks>().DestroyBrick();
                }
            }
            i++;
        }
    }

    public void DestroyBrick()
    {
        StartCoroutine(DestroyBrickTimer());
    }

    IEnumerator DestroyBrickTimer()
    {
        yield return new WaitForSeconds(0.1f);
        particles.Play();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
        yield break;
    }
}
