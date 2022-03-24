using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy : MonoBehaviour
{
    public float speed;
    public float distange;
    public bool movingRight;
    public Transform groundDetection;
    public Transform wall;
    public Transform player;
    public GameObject bullet;
    float turntime;
    float timedelay;
    private Rigidbody2D rb;
    bool run;
    public float rangeShoot;
    private float distToPlayer;

    public float startTimeBtwShots;
    private float timeBtwShots;
    Animator anim;
    bool Attack;
    public ParticleSystem dust;
    private void Start()
    {
        Attack = true;
        timeBtwShots = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        run = true;
        timedelay = 1f;
        turntime = 0f;
        movingRight = true;
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (anim.GetBool("Hit") == true || anim.GetInteger("FallState") == 1)
        {
            run = false;
            Attack = false;
            Debug.Log("hi");
        }
        anim.SetInteger("State", 0);
            if (run)
            {
            Dust();
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distange);
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= rangeShoot && player.position.y >= transform.position.y)
        {
            if ((player.position.x > transform.position.x && movingRight== true && Mathf.Abs(player.position.y - transform.position.y) < 2.5)
                || (player.position.x < transform.position.x && movingRight == false && Mathf.Abs(player.position.y - transform.position.y) < 2.5))
            {
                anim.SetBool("Attack", true);
                run = false;
                if(Attack == true)
                {
                    Shoot();
                }
            }
            else
            {
                anim.SetBool("Attack", false);
                run = true;
            }
        }
        else
        {
            anim.SetBool("Attack", false);
            run = true;
        }
        if (groundInfo.collider == false || (transform.position.x <= wall.transform.position.x && transform.position.y <= wall.transform.position.y))
            {
                turntime = turntime + 1 * Time.deltaTime;
                run = false;
                if (movingRight == true)
                {
                    anim.SetInteger("State", 1);
                    if (turntime >= timedelay)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingRight = false;
                        turntime = 0;
                        run = true;
                    }
                
                }
                if(movingRight == false)
                {
                    anim.SetInteger("State", 1);
                    if (turntime >= timedelay)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                        turntime = 0;
                        run = true;
                    }
                }
            }
    }

    private void Shoot()
    {
           if(timeBtwShots <= 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    void Dust()
    {
        dust.Play();
    }
}
