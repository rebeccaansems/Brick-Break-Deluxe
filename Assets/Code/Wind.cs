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
    private bool wasVisible;

    // Use this for initialization
    void Start()
    {
        direction = Random.Range(-1, 1);
        if (direction == 0)
        {
            direction = 1;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        this.transform.position = new Vector2(this.transform.position.x * direction, this.transform.position.y);
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
    
    private void OnBecameVisible()
    {
        wasVisible = true;
    }

    private void OnBecameInvisible()
    {
        if(wasVisible == true)
        {
            Destroy(this.gameObject);
        }
    }
}
