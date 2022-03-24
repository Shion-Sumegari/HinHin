using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    private Rigidbody2D rb;
    public float falldelay;
    private Animator anim;
    bool check = true;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        y = transform.position.y;
        anim.SetBool("ON-OFF", true);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
            if(collision.collider.CompareTag("Frog"))
            {
            if(check == true)
            {
                StartCoroutine(Down());
                anim.SetBool("ON-OFF", false);
                StartCoroutine(Fall());
            }
            }
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(falldelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(3);
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().isTrigger = false;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        anim.SetBool("ON-OFF", true);
        check = true;
        yield return 0;
    }
    IEnumerator Down()
    {
        yield return new WaitForSeconds(0.01f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        check = false;
        yield return 0;
    }                                                                                                   
}
