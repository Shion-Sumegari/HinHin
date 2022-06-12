using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverUIHandler : MonoBehaviour
{

    [Header("UI Object")]
    [SerializeField] Button BtnClaim;
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup gameOverGroup;
    private Tween fadeTween;
    public float endValue;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn(float duration)
    {
        Fade(endValue, duration, () =>
          {
              gameOverGroup.interactable = true;
              gameOverGroup.blocksRaycasts = true;
          });
        StartCoroutine(Wait());
        
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            gameOverGroup.interactable = false;
            gameOverGroup.blocksRaycasts = false;
        });
    }
    private void Fade(float endValue,float duration, TweenCallback callback)
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false);
        }
        fadeTween = gameOverGroup.DOFade(endValue, duration);
        fadeTween.onComplete += callback;
    } 
    private IEnumerator TestFade()
    {
        yield return new WaitForSeconds(2f);
        FadeOut(1f);
        yield return new WaitForSeconds(3f);
        FadeIn(1f);
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }
}
