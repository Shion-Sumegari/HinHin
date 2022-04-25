using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeControler : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BirdController.controll != null)
        {
            if(BirdController.controll.flag == 1)
            {
                Debug.Log("Olk");
                Destroy(GetComponent<PipeControler>());
            }
        }
        _PipeMove();
    }
    void _PipeMove()
    {
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }
}
