using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead2 : MonoBehaviour
{
    public float speedMax;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    Animator animator;
    bool right = false,up = true,left= false,down= true;
    public float time, timedelay;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        time = 0f;
        timedelay = 1f;
        speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed <= speedMax)
        {
            speed += 0.1f;
        }
        if (transform.position.y >= pos1.transform.position.y && transform.position.x == pos1.transform.position.x && up == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos2.position, speed * Time.deltaTime);
            if (transform.position.y == pos2.transform.position.y)
            {
                speed = 0;
                time = time + 1f * Time.deltaTime;
                if (time >= timedelay)
                {
                    up = false;
                    right = true;
                    left = false;
                    down = false;
                    time = 0f;
                  
                }
            }
        }
        if (transform.position.x >= pos2.transform.position.x && transform.position.y == pos2.transform.position.y && right == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos3.position, speed * Time.deltaTime);
            if (transform.position.x == pos3.transform.position.x)
            {
                speed = 0;
                time = time + 1f * Time.deltaTime;
                if (time >= timedelay)
                {
                    time = 0f;
                    right = false;
                    up = false;
                    down = true;
                    left = false;

                }
            }
        }
        if (transform.position.y <= pos3.transform.position.y && transform.position.x == pos3.transform.position.x &&  down == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos4.position, speed * Time.deltaTime);
            if (transform.position.y == pos4.transform.position.y)
            {
                speed = 0;
                time = time + 1f * Time.deltaTime;
                if (time >= timedelay)
                {
                    time = 0f;
                    up = false;
                    left = true;
                    right = false;
                    down = false;
                }
            }
        }
        if (transform.position.x <= pos4.transform.position.x && transform.position.y == pos4.transform.position.y && left == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos1.position, speed * Time.deltaTime);
            if (transform.position.x == pos1.transform.position.x)
            {
                speed = 0;
                time = time + 1f * Time.deltaTime;
                if (time >= timedelay)
                {
                    time = 0f;
                    right = false;
                    left = false;
                    down = false;
                    up = true;

                }
            }
        }
    }
}
