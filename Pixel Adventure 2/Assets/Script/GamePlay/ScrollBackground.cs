using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
	public float speed;
	public int background_count;
	public float background_height;
	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		//Move down
		transform.Translate(Vector3.down * speed * Time.deltaTime);

		Vector3 pos = transform.position;
		if (pos.y < -background_height)
		{
			pos.y += background_height * background_count;
			transform.position = pos;
		}
	}
}
