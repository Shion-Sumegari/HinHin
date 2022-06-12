using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

public class FXController : MonoBehaviour, IObjectPooler
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate = true;

    public Item.Type m_Type;

    public bool isLoop;

	[SerializeField] Vector3 scale;

	Coroutine stickCoroutine;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while(!isLoop && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if(!ps.IsAlive(true))
			{
				if(OnlyDeactivate)
				{
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
					StopAllCoroutines();
					this.gameObject.SetActive(false);
                    transform.parent = ObjectPooler.Instance.transform;
#endif
                }
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}

    public void OnObjectSpawn(Transform parent, Vector3 offset)
    {
        gameObject.SetActive(true);
		if(parent != null)
        {
			if (stickCoroutine != null) StopAllCoroutines();
			stickCoroutine = StartCoroutine(stickToObject(parent, offset));
		}
		Invoke("PlayFX", 0.03f);
	}

	IEnumerator stickToObject(Transform stickObject, Vector3 offset)
    {
        while (true)
        {
			transform.position = stickObject.position + offset;
			yield return null;
        }
    }

    void PlayFX()
    {
        if (this.GetComponent<ParticleSystem>() != null)
        {
            this.GetComponent<ParticleSystem>().Play();
        }
    }

}
