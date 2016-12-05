using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player, spawnBricks;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < 1)
        {
            this.transform.position = new Vector3(this.transform.position.x, player.transform.position.y - 1, -10);
        }

        if (System.Math.Round(player.transform.position.y, 2) % 2 == 0)
        {
            spawnBricks.GetComponent<SpawnBricks>().SpawnBrick();
        }
    }
}
