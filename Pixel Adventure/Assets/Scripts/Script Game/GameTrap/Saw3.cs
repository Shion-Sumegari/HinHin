using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw3 : MonoBehaviour
{
    public float speed;
    public Transform posSaw1;
    public Transform posSaw2;
    bool right = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= posSaw1.position.x && right == false)
        {
            if (transform.position.x >= posSaw2.position.x)
            {
                right = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, posSaw2.position, speed * Time.deltaTime);
        }
        if (transform.position.x <= posSaw2.position.x && right == true)
        {
            if (transform.position.x <= posSaw1.position.x)
            {
                right = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, posSaw1.position, speed * Time.deltaTime);
        }
    }
}
