using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        FOOD,
        TRAIN,
        COMBAT,
        DIAMOND,
        KEY,
        PLAYER_FX,
        UI_FX
    }

    public static float valueForPoint = 0.02f;

    [SerializeField] int value;

    public Type itemType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getValue()
    {
        return value;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Particle Effect Here
        if (other.CompareTag("Player") && itemType != Type.COMBAT)
        {
            if(itemType== Type.KEY)
            {
                GameManager.Instance.PickupKey();
            }
            gameObject.SetActive(false);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision: " + collision.transform.tag);
    }
}
