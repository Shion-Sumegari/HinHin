using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public Transform Player;
    BoxCollider2D colli;
    // Start is called before the first frame update
    void Start()
    {
        colli = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < (Player.transform.position.y))
        {
            colli.isTrigger = false;
        }
        if (transform.position.y > (Player.transform.position.y - 0.2f)){
            colli.isTrigger = true;
 
        }
    }
}
