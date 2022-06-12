using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionHelper
{
    public static IEnumerator StartAction(UnityAction action, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (action != null) action.Invoke();
    }
}
