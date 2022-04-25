using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public static BirdController controll;
    public float force;
    private Rigidbody2D Rb;
    private Animator anim;


    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip flyClip, pingClip, diedClip;

    public float flag = 0;
    public int score = 0;
    private GameObject spawner;
    private bool isAlive;
    private void Awake()
    {
        isAlive = true;
        Rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _MakeInstance();
        spawner = GameObject.Find("Spawn Pipe");
    }
     void _MakeInstance()
    {
           if(controll == null)
        {
            controll = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        _BirdMove();
    }
    void _BirdMove()
    {
        if (isAlive && Input.GetMouseButtonDown(0))
        {
            Rb.velocity = new Vector2(Rb.velocity.x, force);
            audioSource.PlayOneShot(flyClip);
        }
        if(Rb.velocity.y > 0)
        {
            float angel = 0;
            angel = Mathf.Lerp(0, 45, Rb.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angel);
        } else if (Rb.velocity.y == 0)
        {
            float angel = 0;
            angel = Mathf.Lerp(0, -45, Rb.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (Rb.velocity.y < 0)
        {
            float angel = 0;
            angel = Mathf.Lerp(0, -90, -Rb.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angel);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Pipe")
        {
            score++;
            if(PlayControl.instance != null)
            {
                PlayControl.instance._SetScore(score);
            }
            audioSource.PlayOneShot(pingClip);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
          if(collision.gameObject.tag == "PipeWall" || collision.gameObject.tag == "Ground")
        {
            flag = 1;
            if (isAlive)
            {
                isAlive = false;
                Destroy(spawner);
                audioSource.PlayOneShot(diedClip);
                anim.SetTrigger("Died");
                if (Scroll.instance != null)
                {
                    Scroll.instance.speed = 0f;
                }
            }
            if(PlayControl.instance != null)
            {
                PlayControl.instance._GameOver(score);
            }
        }
    }
}
