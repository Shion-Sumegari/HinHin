using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw1 : MonoBehaviour
{
    public float speed;
    public Transform posSaw1;
    public Transform posSaw2;
    public Transform posSaw3;
    public Transform posSaw4;
    bool turnright, up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= posSaw1.position.x && transform.position.y == posSaw1.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, posSaw2.position, speed * Time.deltaTime);
        }
        if(transform.position.y <= posSaw2.position.y && transform.position.x == posSaw2.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, posSaw3.position, speed * Time.deltaTime);
        }
        if (transform.position.x <= posSaw3.position.x && transform.position.y == posSaw3.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, posSaw4.position, speed * Time.deltaTime);
        }
        if (transform.position.y >= posSaw4.position.y && transform.position.x == posSaw4.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, posSaw1.position, speed * Time.deltaTime);
        }
    }
}
