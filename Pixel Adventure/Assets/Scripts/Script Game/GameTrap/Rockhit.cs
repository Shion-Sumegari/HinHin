using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockhit : MonoBehaviour
{
    public Transform RockHead1;
    public Transform RockHead;
    public Transform RockHead2;
    public Transform RockHead3;
    Animator anim;
    Rigidbody2D Rb;
    BoxCollider2D coli;
    public Transform obj1, obj2, obj3, obj4;
    public float pushBackForce;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        coli = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //x: 5 ->6 ; y: -4.6 -> 3.6
        //x: -4 -> -3  ; y: -4.6 -> -3.6
        if(transform.position.y > -4.6 && transform.position.y < -3.6)
        {
            if( transform.position.x >5 && transform.position.x < 6 && RockHead.transform.position.x > 5)
            {
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
            if (transform.position.x > -4 && transform.position.x < -3 && RockHead.transform.position.x < -3)
            {
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
      
        }

        //y: -7.6 -> -6.4
        if( transform.position.x >=-7.6 && transform.position.x <= -6.4 && RockHead1.transform.position.y <= -4 && transform.position.y < RockHead1.transform.position.y)
        {
            Debug.Log("ok");
            Rb.bodyType = RigidbodyType2D.Static;
            coli.isTrigger = true;
            anim.SetTrigger("Hit");
            anim.SetBool("Isdead", true);
        }
        // Trên nóc của rock
      if(transform.parent != null && transform.parent.name == "FeetinGround1")
            {
                if(RockHead1.transform.position.y > 3)
                {
                Debug.Log(transform.parent.name);
                    Debug.Log("ok");
                    Rb.bodyType = RigidbodyType2D.Static;
                    coli.isTrigger = true;
                    anim.SetTrigger("Hit");
                    anim.SetBool("Isdead", true);
                }
            }
        //x: -2.4 -> -1.6  y: -1 -> 0
        if (transform.position.x >= -2.4 && transform.position.x <= -1.6 && transform.position.y >= -1 && transform.position.y <=0)
        {
            if(RockHead3.position == obj1.position || RockHead2.position == obj1.position)
            {
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
        }
        //x: 7 -> 8  y: 2.8 -> 4
        if (transform.position.x >= 7 && transform.position.x <= 8 && transform.position.y >= 2.8 && transform.position.y <= 4)
        {
            if (RockHead3.position == obj2.position || RockHead2.position == obj2.position)
            {
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
        }
        //x: 7 -> 8  y: -1 -> 0
        if (transform.position.x >= 7 && transform.position.x <= 8 && transform.position.y >= -1 && transform.position.y <= 0)
        {
            if (RockHead3.position == obj3.position || RockHead2.position == obj3.position)
            {
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
        }
        if (transform.parent != null && transform.parent.name == "FeetinGround2")
        {
            if (RockHead2.transform.position == obj4.position )
            {
                Debug.Log(transform.parent.name);
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
        }
        if (transform.parent != null && transform.parent.name == "FeetinGround3")
        {
            if (RockHead3.transform.position == obj4.position)
            {
                Debug.Log(transform.parent.name);
                Debug.Log("ok");
                Rb.bodyType = RigidbodyType2D.Static;
                coli.isTrigger = true;
                anim.SetTrigger("Hit");
                anim.SetBool("Isdead", true);
            }
        }
    }
}
