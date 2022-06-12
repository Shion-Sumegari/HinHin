using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    Coroutine timeScaleChangingCoroutine;

    public void VibrationWithDelay(long milliseconds, float timer) // #param1 Duration, #param2 Delay
    {
        StartCoroutine(VibrateDelay(milliseconds, timer));
    }

    IEnumerator VibrateDelay(long milliseconds, float timer)
    {
        yield return new WaitForSeconds(timer);
        Vibration.Vibrate(milliseconds);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.L))
        {
            Handheld.Vibrate();
        }
    }

    public void ChangeTimeScale(float targetTimeScale, float speed, float delay)
    {
        if (timeScaleChangingCoroutine != null) StopCoroutine(timeScaleChangingCoroutine);
        timeScaleChangingCoroutine = StartCoroutine(IEChangeTimeScale(targetTimeScale, speed, delay));
    }

    IEnumerator IEChangeTimeScale(float targetTimeScale, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);

        while (Time.timeScale != targetTimeScale)
        {
            
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, Time.deltaTime * speed);
            LogUtils.Log("time Scale: " + Time.timeScale);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Slowmotion(float howSlow, float duration, float speed, float delay)
    {
        if (timeScaleChangingCoroutine != null) StopCoroutine(timeScaleChangingCoroutine);
        timeScaleChangingCoroutine = StartCoroutine(IESlowMotion(howSlow, duration, speed, delay));
    }

    IEnumerator IESlowMotion(float howSlow, float duration, float speed, float delay)
    {
        var oldTimeScale = Time.timeScale;
        yield return new WaitForSeconds(delay);
        while (Time.timeScale != howSlow)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, howSlow, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(duration);
        while (Time.timeScale != oldTimeScale)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, oldTimeScale, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}