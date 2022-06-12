using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRoadController : MonoBehaviour
{
    public List<PointPlaneController> pointPlaneControllers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        foreach (var item in pointPlaneControllers)
        {
            if(item != null) item.isReached = false;
        }
    }
}
