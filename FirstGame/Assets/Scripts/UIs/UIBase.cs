using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIBase : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected virtual void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void TurnOn(float duration = 0.65f)
    {
        gameObject.SetActive(true);
        StartCoroutine(_OnTurnOn(duration));
    }
    public virtual void TurnOff(float duration = 0.65f)
    {
        StartCoroutine(_OnTurnOff(duration));
    }
    IEnumerator _OnTurnOn(float duration)
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.65f);
        yield return null;
    }
    IEnumerator _OnTurnOff(float duration)
    {
        canvasGroup.DOFade(0, 0.65f);
        yield return new WaitForSeconds(0.65f);
        gameObject.SetActive(false);
    }
}
