using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    Touch touch;
    [SerializeField] Transform testPos;
    [SerializeField] Vector3 maxX, minX;
    [SerializeField] float speedModifier;
    [SerializeField] float rotationSpeed;

    Vector2 oldTouchPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateByMove();
    }

    void RotateByMove()
    {
        Vector3 targetDirection = (new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f) - transform.position).normalized;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                oldTouchPos = touch.position;
                Debug.Log("Began: " + oldTouchPos);
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                var newTouchPos = touch.position;

                float v = newTouchPos.y - oldTouchPos.y;
                float h = newTouchPos.x - oldTouchPos.x;

                Vector3 forward = targetDirection;
                forward.y = 0;
                forward = forward.normalized;

                Vector3 right = new Vector3(forward.z, 0, forward.x);

                targetDirection = h * right * 0.4f + Mathf.Abs(v) * forward;
            }

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
        Debug.DrawRay(transform.position, targetDirection, Color.black, Time.deltaTime);
    }
}
