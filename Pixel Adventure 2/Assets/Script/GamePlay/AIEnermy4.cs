using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnermy4 : MonoBehaviour
{
    public float speed;
    public Transform dot, dot1;
    private Rigidbody2D rb;
    bool run;

    Animator anim;
    public ParticleSystem dust;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        run = true;
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
            if (run)
            {
            Dust();
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        
        if ((transform.position.x <= dot.transform.position.x && transform.position.y == dot.transform.position.y))
            {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if((transform.position.x >= dot1.transform.position.x && transform.position.y == dot1.transform.position.y))
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    void Dust()
    {
        dust.Play();
    }
}
