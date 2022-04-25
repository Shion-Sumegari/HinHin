using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pipe;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawner());
    }
    IEnumerator spawner()
    {
        yield return new WaitForSeconds(2);
        Vector3 temp = pipe.transform.position;
        temp.y = Random.Range(-2.5f, 2.5f);
        Instantiate(pipe, temp, Quaternion.identity);
        StartCoroutine(spawner());
    }
}
