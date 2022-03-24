using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float moveSpeed;
    public float lefltAngle;
    public float rightAngle;
    Rigidbody2D Rb;

    bool movingClockwise;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        movingClockwise = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }
    public void ChangeMoveDir()
    {
        if(transform.rotation.z > rightAngle)
        {
            movingClockwise = false;
        }
        if(transform.rotation.z < lefltAngle)
        {
            movingClockwise = true;
        }
    } 

    public void Move()
    {
        ChangeMoveDir();
        if (movingClockwise)
        {
            Rb.angularVelocity = moveSpeed; 
        }
        if(!movingClockwise)
        {
            Rb.angularVelocity = -1 * moveSpeed;
        }
    }
}
