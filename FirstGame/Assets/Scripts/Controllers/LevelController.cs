using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    int index;

    public Vector3 startPoint, endPoint;

    public GameObject endChallenge;

    public LevelEnd levelEndType;

    public float startScale;

    [Range(1, 100)] public int rewardUnlock;
}
