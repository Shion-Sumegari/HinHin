using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead1 : MonoBehaviour
{
    public float speedMax;
    public Transform posSaw1;
    public Transform posSaw2;
    Animator animator;
    bool right = false;
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
            if(speed <= speedMax)
        {
            speed += 0.1f;
            animator.SetFloat("Move", speed);
        }
            if (transform.position.y >= posSaw1.position.y && right == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, posSaw2.position, speed * Time.deltaTime);
                if (transform.position.y == posSaw2.position.y)
                {
                speed = 0;
                animator.SetFloat("Move", speed);
                time = time + 1f * Time.deltaTime;
                    if(time >= timedelay)
                {
                    right = true;
                    time = 0f;
                    
                }
                }
            }
            if (transform.position.y <= posSaw2.position.y && right == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, posSaw1.position, speed * Time.deltaTime);
                if (transform.position.y == posSaw1.position.y)
                {
                speed = 0;
                animator.SetFloat("Move", speed);
                time = time + 1f * Time.deltaTime;
                if (time >= timedelay)
                {
                    right = false;
                    time = 0f;
                  
                }
                }
            }
    }
}
