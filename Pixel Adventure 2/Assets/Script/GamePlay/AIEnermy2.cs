using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy2 : MonoBehaviour
{
    public Transform player;
    public GameObject bullet;
    private Rigidbody2D rb;
    public float rangeShoot;
    private float distToPlayer;

    public float startTimeBtwShots;
    private float timeBtwShots;
    Animator anim;
    bool Attack;
    private void Start()
    {
        Attack = true;
        timeBtwShots = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (anim.GetBool("Hit") == true || anim.GetInteger("FallState") == 1)
        {
            Attack = false;
            Debug.Log("hi");
        }

        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= rangeShoot && (player.position.y +1f - transform.position.y <2f) && (player.position.y + 1f - transform.position.y > 0f))
        {
            anim.SetBool("Attack", true);
            if (Attack == true)
            {
                Shoot();
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    private void Shoot()
    {
        
        if (timeBtwShots <= 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
