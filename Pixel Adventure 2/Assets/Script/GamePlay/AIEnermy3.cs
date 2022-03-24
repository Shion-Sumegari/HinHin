using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy3 : MonoBehaviour
{
    public float speed;
    public float distange;
    public bool movingRight;
    public Transform groundDetection;
    public Transform wall, wall1;
    public Transform player;
    float turntime;
    float timedelay;
    private Rigidbody2D rb;
    bool run;

    Animator anim;
    public ParticleSystem dust;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        run = true;
        timedelay = 1f;
        turntime = 0f;
        movingRight = true;
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        anim.SetInteger("State", 1);
        if(anim.GetBool("Shell") == true)
        {
            speed = 10;
            turntime = timedelay;
        }
        if(anim.GetBool("Fall") == true)
        {
            run = false;
        }
            if (run)
            {
            Dust();
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distange);
        if (groundInfo.collider == false || (transform.position.x >= wall.transform.position.x && transform.position.y <= wall.transform.position.y) || (transform.position.x <= wall1.transform.position.x && transform.position.y <= wall1.transform.position.y))
            {
            turntime = turntime + 1 * Time.deltaTime;
                run = false;
                if (movingRight == true)
                {
                    anim.SetInteger("State", 0);
                        if (turntime >= timedelay)
                         {
                         if (anim.GetBool("Shell") == true)
                         {
                            anim.SetBool("Wall", true);
                         }
                    transform.eulerAngles = new Vector3(0, -180, 0);
                            movingRight = false;
                            turntime = 0;
                            run = true;
                         }
                }
                if(movingRight == false)
                {
                    anim.SetInteger("State", 0);
                if (turntime >= timedelay)
                {
                    if (turntime >= timedelay)
                    {
                        if (anim.GetBool("Shell") == true)
                        {
                            anim.SetBool("Wall", true);
                        }
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                        turntime = 0;
                        run = true;

                    }
                }
                }
        }
        else
        {
            if (anim.GetBool("Shell") == true)
            {
                anim.SetBool("Wall", false);
            }
        }
    }

    void Dust()
    {
        dust.Play();
    }
}
