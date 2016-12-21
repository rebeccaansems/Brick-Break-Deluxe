using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenBricks : MonoBehaviour
{
    public MainScreenBrickSpawn spawner;
    public ParticleSystem particles;

    private Vector2 location;

    void Start()
    {
        location = this.transform.position;
        int secondsUntilDestroy = Random.Range(12, 60);
        StartCoroutine(DestroyBrickTimer(secondsUntilDestroy));
    }

    IEnumerator DestroyBrickTimer(int secondsUntilDestroy)
    {
        yield return new WaitForSeconds(secondsUntilDestroy);

        particles.Play();
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        yield return new WaitForSeconds(5);

        spawner.createNewBrick(location);
        Destroy(this.gameObject);
        StopCoroutine(DestroyBrickTimer(100));
    }
}
