using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector2 target;
    private Transform player;
    public GameObject piece1, piece2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Frog").transform;
        if (transform.position.x < player.position.x)
        {
            target = new Vector2(100, transform.position.y);
        }
        if(transform.position.x > player.position.x)
        {
            target = new Vector2(-100, transform.position.y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Frog")){
            Destroy(gameObject);
        }
        if (collision.CompareTag("Cherry"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Frog"))
        {
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Cherry"))
        {
            Destroy(gameObject);
        }
    }
}
