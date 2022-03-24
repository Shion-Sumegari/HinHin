using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hit : MonoBehaviour
{
    Animator anim;
    BoxCollider2D coli;
    public float pushBackForce;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        coli = transform.gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            coli.isTrigger = true;
            anim.SetTrigger("Hit");
            Pushback(collision.transform);
            anim.SetBool("Isdead", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            coli.isTrigger = true;
            anim.SetTrigger("Hit");
            Pushback(collision.transform);
            anim.SetBool("Isdead", true);
        }
    }
    void Pushback(Transform trap)
    {
        float Plus;
        Vector2 pushDirrection = new Vector2(0, 1f);
        pushDirrection *= pushBackForce;
        Rigidbody2D pushRb = transform.gameObject.GetComponent<Rigidbody2D>();
        if(transform.position.x != trap.position.x)
        {
            Plus = (transform.position.x - trap.position.x) / Mathf.Abs(transform.position.x - trap.position.x);
        }
        else
        {
            Plus = 0;
        }
        pushRb.velocity = new Vector2(pushRb.velocity.x + 5f, pushRb.velocity.y);
        pushRb.position = new Vector3(transform.position.x + (Plus* 0.8f), transform.position.y, transform.position.z);
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(pushDirrection, ForceMode2D.Impulse);
        pushRb.bodyType = RigidbodyType2D.Static;
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
