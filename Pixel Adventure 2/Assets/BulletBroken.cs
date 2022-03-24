using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBroken : MonoBehaviour
{
    public GameObject bulletBroken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
          if(other.collider.tag == "Cherry")
        {
            Break();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Cherry")
        {
            Break();
        }
    }
    void Break()
    {
        Destroy(this.gameObject);
        Instantiate(bulletBroken, transform.position, Quaternion.identity);
    }
}
