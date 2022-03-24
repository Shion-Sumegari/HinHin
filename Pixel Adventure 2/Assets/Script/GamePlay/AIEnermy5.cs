using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class AIEnermy5 : MonoBehaviour
{
    public float speed;
    public Transform player;
    public Transform wall, wall1;
    private Rigidbody2D rb;
    bool run;
    public float range;
    private float distToPlayer;
    Animator anim;
    bool Attack;
    public ParticleSystem dust;
    private Vector2 target;
    private bool targetCheck;
    private float time, timedelay;
    public float pushBackForce;
    private bool pushCheck;
    private bool wallCheck;
    private void Start()
    {
        Attack = true;
        rb = GetComponent<Rigidbody2D>();
        run = false;
        anim = GetComponent<Animator>();
        targetCheck = true;
        target = new Vector2(transform.position.x,transform.position.y);
        timedelay = 2;
        time = 0;
        speed = 0;
        pushCheck = true;
        wallCheck = true;
    }
    private void Update()
    {
        if (anim.GetBool("Hit") == true || anim.GetInteger("FallState") == 1)
          {
              run = false;
              Debug.Log("hi");
          }
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if(run == true  )
        {
            if (speed <= 20)
            {
                speed += 0.5f;
            }
            Dust();
            anim.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Run", false);
        }
       
        if(transform.position.x <= (wall.position.x +0.00001f) || transform.position.x >= (wall1.position.x))
        {
            speed = 0;
            time = time + Time.deltaTime;
            if(wallCheck == true)
            {
                anim.SetBool("Wall", true);
                //CameraShaker.Instance.ShakeOnce(1f, 2f, 0.5f, 0.2f);
                wallCheck = false;
            }
            anim.SetBool("Run", false);
            StartCoroutine(wait());
            if(pushCheck == true)
            {
                pushBackEnemy();
            }
            if (time >= timedelay)
            {      
                run = false;
                targetCheck = true;
                time = 0;
                pushCheck = true;
                wallCheck = true;
            }
        }
        if (player.position.x > transform.position.x && run == false)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        if (player.position.x < transform.position.x && run == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (distToPlayer <= range && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.5)
        {
            run = true;
            if (player.transform.position.x >= transform.position.x && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.5 && targetCheck == true)
            {
                target = new Vector2(wall1.transform.position.x, wall1.transform.position.y);
                targetCheck = false;
            }
            if (player.transform.position.x < transform.position.x && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.5 && targetCheck == true)
            {
                target = new Vector2(wall.transform.position.x,wall.transform.position.y);
                targetCheck = false;
            }
        }
        else
        {
                 if(!run)
            {
                pushCheck = false;
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.0001f);
        anim.SetBool("Wall", false);
        yield return 0;
    }
    void pushBackEnemy()
    {
        pushCheck = false;
        Vector2 pushDirection = new Vector2(0,0);
        if (transform.position.x <= wall.position.x)
        {
            pushDirection = new Vector2(-1f, 0.5f);
        } if(transform.position.x >= wall1.position.x)
        {
            pushDirection = new Vector2(1f, 0.5f);
        }
        pushDirection *= pushBackForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection, ForceMode2D.Impulse);
    }
    void Dust()
    {
        dust.Play();
    }
}
