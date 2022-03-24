using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Frog"))
        {
            StartCoroutine(Wait());
        }
        if (collision.collider.CompareTag("Cherry"))
        {
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        yield return 0;
    }
}
