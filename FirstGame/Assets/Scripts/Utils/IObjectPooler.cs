using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPooler
{
    void OnObjectSpawn(Transform parent, Vector3 offset);
}
