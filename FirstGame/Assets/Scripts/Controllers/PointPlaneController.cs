using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlaneController : MonoBehaviour
{
    public float pointX;

    public Material disableMaterial, enableMaterial;

    private bool m_isReach;
    public bool isReached { get { return m_isReach; } set { m_isReach = value; SwapMaterial(); } } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwapMaterial()
    {
        GetComponent<MeshRenderer>().material = m_isReach ? enableMaterial : disableMaterial;
    }
}
