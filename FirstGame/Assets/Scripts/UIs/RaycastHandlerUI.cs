using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHandlerUI : MonoBehaviour
{
    [SerializeField] Camera targetCam;
    [SerializeField] LayerMask targetMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector3 origin = targetCam.ScreenToWorldPoint(touch.position);
            //RaycastHit hit;
            //if (Physics.Raycast(origin, targetCam.transform.forward, out hit, Mathf.Infinity, 5))
            //{
            //    Debug.Log(hit.collider.name + " " + hit.collider.gameObject.layer);
            //    //Debug.DrawLine(origin, hit.point, Color.red);
            //    Debug.DrawLine(origin, origin + targetCam.transform.forward * 1000, Color.red);


            //}
            //else
            //{
            //    Debug.DrawLine(origin, origin + targetCam.transform.forward * 1000,Color.red);
            //}
            RaycastHit[] hits = Physics.RaycastAll(origin, targetCam.transform.forward, Mathf.Infinity,targetMask,QueryTriggerInteraction.Collide);
            for(int i = 0; i < hits.Length; i++)
            {
                //if (hits[i].collider.name.Equals("UI_Chest"))
                //{
                //    Debug.Log(hits[i].collider.name);
                //}
                IClickRaycastHandler clickRaycastHandler = hits[i].collider.GetComponent<IClickRaycastHandler>();
                if (clickRaycastHandler!=null)
                {
                    clickRaycastHandler.OnClick();
                }
            }
        }   
    }
}
