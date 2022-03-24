using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw2 : MonoBehaviour
{
    public float speed;
    public Transform posSaw1;
    public Transform posSaw2;
    bool down = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= posSaw1.position.y && down == false)
        {
            if(transform.position.y >= posSaw2.position.y)
            {
                down = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, posSaw2.position, speed * Time.deltaTime);
        }
        if(transform.position.y <= posSaw2.position.y && down == true)
        {
            if (transform.position.y <= posSaw1.position.y)
            {
                down = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, posSaw1.position, speed * Time.deltaTime);
        }
    }
}
