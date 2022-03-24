using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy6 : MonoBehaviour
{
    public float speed;
    public bool movingUp;
    public Transform player;
    public Transform fly, fly1;
    private Vector2 target;
    private Rigidbody2D rb;
    public bool check, reset;
    public float ground;

    Animator anim;
    public ParticleSystem dust;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingUp = true;
        anim = GetComponent<Animator>();
        target = new Vector2(transform.position.x, ground);
        check = false;
        reset = false;

    }
    private void Update()
    {
        Dust();
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.5f && player.transform.position.y < transform.position.y)
        {
            check = true;
            reset = false;
            anim.SetInteger("State", 1);
        }
        if (check == true)
        {
            speed = 10;
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position.y == ground)
            {
                StartCoroutine(wait());
            }
            else
            {
                check = true;
            }
        }
        else
        {
            if(check == false && reset == false)
            {
                speed = 1;
                anim.SetInteger("State", 0);
                transform.position = Vector2.MoveTowards(transform.position, fly.position, speed * Time.deltaTime);
                if(transform.position.y == fly.position.y)
                {
                    speed = 0.3f;
                    reset = true;
                }
            }
            if (transform.position.x == fly.transform.position.x && transform.position.y >= fly.transform.position.y && movingUp == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, fly1.position, speed * Time.deltaTime);
                if (transform.position.y == fly1.position.y)
                {
                    movingUp = false;
                }
            }
            if (transform.position.x == fly1.transform.position.x && transform.position.y <= fly1.transform.position.y && movingUp == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, fly.position, speed * Time.deltaTime);
                if (transform.position.y == fly.position.y)
                {
                    movingUp = true;
                }
            }
        }

    }
    IEnumerator wait()
    {
        anim.SetInteger("State", 2);
        yield return new WaitForSeconds(0.8f);
        check = false;
        yield return 0;
    }

    void Dust()
    {
        dust.Play();
    }
}
