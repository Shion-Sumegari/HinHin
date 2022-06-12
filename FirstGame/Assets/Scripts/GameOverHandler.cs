using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup gameOverGroup;
    [SerializeField] CanvasGroup BtnClaim;
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
            BtnClaim.DOFade(1f, duration);
        });
    }

    public void FadeOut(float duration)
    {
        BtnClaim.DOFade(0f, duration);
        Fade(0f, duration, () =>
        {
            gameOverGroup.interactable = false;
            gameOverGroup.blocksRaycasts = false;
        });
    }
    private void Fade(float endValue, float duration, TweenCallback callback)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = gameOverGroup.DOFade(endValue, duration);
        fadeTween.onComplete += callback;
    }
    private IEnumerator TestFade()
    {
        yield return new WaitForSeconds(2f);
        FadeIn(2);
        yield return new WaitForSeconds(5f);
        FadeOut(2f);
    }
}