using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{

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
        Destroy(this.gameObject);
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
        Destroy(this.gameObject);
        yield break;
    }
}
