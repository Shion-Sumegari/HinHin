using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HitEnemy1 : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    Animator anim;
    BoxCollider2D box;
    Rigidbody2D rb;
    public float pushBackForce;
    private int countHit;
    public GameObject Snail;
    // Start is called before the first frame update
    void Start()
    {
        anim = enemy.GetComponent<Animator>();
        box = enemy.GetComponent<BoxCollider2D>();
        rb = enemy.GetComponent<Rigidbody2D>();
        countHit = 2;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Frog")){
            if(countHit == 2)
            {
                Debug.Log("Hit");
                CameraShaker.Instance.ShakeOnce(1f, 2f, 0.5f, 0.2f);
                anim.SetBool("Shell", true);
                pushBack(player.transform);
                Instantiate(Snail, transform.position, Quaternion.identity);
            }
            if(countHit  == 1)
            {
                pushBack(player.transform);
                anim.SetTrigger("Top");
                StartCoroutine(wait());
                
            }
            countHit--;
        }
    }
    void pushBack(Transform pushedObject)
    {
        Vector2 pushDirection = new Vector2(0,(pushedObject.position.y - enemy.transform.position.y)).normalized;
        pushDirection *= pushBackForce;
        Rigidbody2D pushRB = pushedObject.gameObject.GetComponent<Rigidbody2D>();
        pushRB.velocity = Vector2.zero;
        pushRB.AddForce(pushDirection,ForceMode2D.Impulse);
    }
    void pushBackEnemy(Transform pushedObject)
    {
        Vector2 pushDirection = new Vector2(0.2f, 0.4f);
        pushDirection *= pushBackForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection, ForceMode2D.Impulse);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Fall", true);
        yield return new WaitForSeconds(0.2f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        box.isTrigger = true;
        transform.GetComponent<BoxCollider2D>().isTrigger = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        pushBackEnemy(enemy.transform);
        yield return 0;
    }
}
