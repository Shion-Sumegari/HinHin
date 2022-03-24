using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collected : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Frog"))
        {
            StartCoroutine(Collect());
            
        }
    }
    IEnumerator Collect()
    {
        anim.SetBool("Collected", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Collected", false);
        yield return 0;
    }
}
