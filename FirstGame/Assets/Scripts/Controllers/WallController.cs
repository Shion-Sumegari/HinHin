using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] GameObject unbreakableWall;

    [SerializeField] GameObject breakBricks;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    Collider[] colliders = Physics.OverlapSphere(hit.point, 3f);

            //    foreach (Collider exHit in colliders)
            //    {
            //        Rigidbody rb = exHit.GetComponent<Rigidbody>();

            //        if (rb != null)
            //        {
            //            Debug.Log("Brick: " + rb.gameObject.name);
            //            rb.AddExplosionForce(300f, hit.point, 3f, 1.0F);
            //        }

            //    }
            //}
            //Debug.Log("Fire");

            

        }
    }

    public void IsBreakable(bool isBreak)
    {
        unbreakableWall.SetActive(!isBreak);
        breakBricks.SetActive(isBreak);
    }

    public void Shake()
    {
        GetComponent<ObjectShake>().Shake();
    }
}
