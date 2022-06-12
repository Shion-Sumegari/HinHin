using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundStanding : MonoBehaviour
{
    [SerializeField] bool isKeepStandingOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeepStandingOnGround)
        {
            KeepOnGround();
        }
    }

    void KeepOnGround()
    {
        RaycastHit hit;
        Vector3 startRay = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        if (Physics.Raycast(startRay, -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                Debug.DrawLine(startRay, hit.point, Color.red);
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.1f, transform.position.z);
            }
        }
    }
}
