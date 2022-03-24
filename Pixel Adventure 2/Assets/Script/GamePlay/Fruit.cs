using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int Count;
    public string level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit") && collision != null)
        {
            StartCoroutine(Wait(collision));
        }
    }
    IEnumerator Wait(Collider2D collider)
    {
        if(collider != null)
        {
            Animator anim = collider.gameObject.GetComponent<Animator>();
                Count--;
                yield return new WaitForSeconds(0f);
                Destroy(collider.gameObject);
            yield return 0;
        }
        else
        {
            yield return 0;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Count == 0)
        {
            StartCoroutine(wait());
        }
    }

    [System.Obsolete]
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
        Application.LoadLevel(level);
        yield return 0;
    }
}
