using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Apple"))
        {
 
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Orange"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PineAplle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
