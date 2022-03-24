using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetinRock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Pink")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
         if(collision.gameObject.name == "Pink")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
