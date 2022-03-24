using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRock : MonoBehaviour
{
    Animator anim;
    public Transform Wall1, Wall2, Wall3, Wall4;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall") && (transform.position.x - Wall1.position.x <= 5f))
        {
            StartCoroutine(LeftHit());
        }
        if (collision.gameObject.CompareTag("wall") && (-transform.position.x + Wall2.position.x <= 5f))
        {
            StartCoroutine(RightHit());
        }
        if (collision.gameObject.CompareTag("wall1") && (transform.position.y - Wall3.position.y <= 5f))
        {
            StartCoroutine(BottomHit());
        }
        if (collision.gameObject.CompareTag("wall1") && (-transform.position.y + Wall4.position.y <= 5f))
        {
            StartCoroutine(TopHit());
        }
    }
    IEnumerator LeftHit()
    {
        anim.SetBool("Lefthit", true);
        yield return new WaitForSeconds(0f);
        anim.SetBool("Lefthit", false);
        yield return 0;
    }
    IEnumerator RightHit()
    {
        anim.SetBool("Righthit", true);
        yield return new WaitForSeconds(0f);
        anim.SetBool("Righthit", false);
        yield return 0;
    }
    IEnumerator BottomHit()
    {
        anim.SetBool("Bottomhit", true);
        yield return new WaitForSeconds(0f);
        anim.SetBool("Bottomhit", false);
        yield return 0;
    }
    IEnumerator TopHit()
    {
        anim.SetBool("Tophit", true);
        yield return new WaitForSeconds(0f);
        anim.SetBool("Tophit", false);
        yield return 0;
    }
}
