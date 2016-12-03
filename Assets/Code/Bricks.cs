using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public ParticleSystem particles;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        StartCoroutine(DestroyBrick());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            StartCoroutine(DestroyBrick());
        }
    }

    IEnumerator DestroyBrick()
    {
        yield return new WaitForSeconds(Random.Range(0.01f, 1.0f));
        particles.Play();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
        yield break;
    }
}
