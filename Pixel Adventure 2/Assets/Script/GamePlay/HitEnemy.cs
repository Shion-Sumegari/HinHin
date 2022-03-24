using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HitEnemy : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    Animator anim;
    BoxCollider2D box;
    Rigidbody2D rb;
    public float pushBackForce;
    // Start is called before the first frame update
    void Start()
    {
        anim = enemy.GetComponent<Animator>();
        box = enemy.GetComponent<BoxCollider2D>();
        rb = enemy.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Frog")){
            Debug.Log("Hit");
            CameraShaker.Instance.ShakeOnce(1f, 2f, 0.5f, 0.2f);
            anim.SetBool("Hit", true);
            pushBack(player.transform);
            StartCoroutine(wait());
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
        anim.SetInteger("FallState", 1);
        yield return new WaitForSeconds(0.2f);
        box.isTrigger = true;
        transform.GetComponent<BoxCollider2D>().isTrigger = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        pushBackEnemy(enemy.transform);
        yield return 0;
    }
}
