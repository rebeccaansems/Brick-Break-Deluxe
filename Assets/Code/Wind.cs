using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public GameObject player;
    [Tooltip("Left is 1, Right is -1")]
    public int direction;
    public float speed;

    private bool playerInWindTunnel = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInWindTunnel == true)
        {
            player.GetComponent<Rigidbody2D>().AddForce(transform.right * direction * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerInWindTunnel == false)
        {
            playerInWindTunnel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerInWindTunnel == true)
        {
            playerInWindTunnel = false;
        }
    }
}
