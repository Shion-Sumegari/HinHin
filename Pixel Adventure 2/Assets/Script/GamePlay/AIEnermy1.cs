using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy1 : MonoBehaviour
{
    public float speed;
    public float distange;
    public Transform groundDetection;
    public Transform wall;
    public Transform player;
    private Rigidbody2D rb;
    bool run;
    public float range;
    private float distToPlayer;
    Animator anim;
    bool Attack;
    public ParticleSystem dust;
    private Vector2 target;
    private void Start()
    {
        Attack = true;
        rb = GetComponent<Rigidbody2D>();
        run = true;
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (anim.GetBool("Hit") == true || anim.GetInteger("FallState") == 1)
          {
              run = false;
              Debug.Log("hi");
          }
        target = new Vector2(player.transform.position.x, transform.position.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distange);
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (run)
            {
                Dust();
            if (groundInfo.collider == false || (transform.position.x <= wall.transform.position.x && transform.position.y <= wall.transform.position.y && player.transform.position.x < transform.position.x) || transform.position.x == player.transform.position.x)
            {
                run = false;
                anim.SetBool("Run", false);
            }
            else
            {
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            
        }
        
        if (distToPlayer <= range && Mathf.Abs(transform.position.y - player.transform.position.y) < 2 )
        {
            if (groundInfo.collider == false || (transform.position.x <= wall.transform.position.x && transform.position.y <= wall.transform.position.y && player.transform.position.x < transform.position.x) || transform.position.x == player.transform.position.x)
            {
                run = false;
                anim.SetBool("Run", false);
            }
            else
            {
                run = true;
                anim.SetBool("Run", true);
            }

        }
        else
        {
            run = false;
            anim.SetBool("Run", false);
        }
        if (player.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        } 
        if(player.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    void Dust()
    {
        dust.Play();
    }
}
