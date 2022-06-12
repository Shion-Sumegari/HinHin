using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimations : MonoBehaviour
{
    [SerializeField] bool _isSpinningAroundYAxis;
    [SerializeField] float _spinningSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSpinningAroundYAxis)
        {
            transform.RotateAround(transform.position, Vector3.up, _spinningSpeed * Time.deltaTime);
        }
    }
}
