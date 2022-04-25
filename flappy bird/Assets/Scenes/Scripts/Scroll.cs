using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	public static Scroll instance;
	public float speed = 4f;
	private Vector3 StartPos;
	// Start is called before the first frame update
	void Start()
	{
		StartPos = transform.position;
		_MakeInsstance();
	}
	void _MakeInsstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.left * speed );
		if(transform.position.x < -4)
        {
			transform.position = StartPos;
        }
	}
}
