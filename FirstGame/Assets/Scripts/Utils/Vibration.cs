﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
        {
            vibrator.Call("vibrate");
        }
        else
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }


    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
        {
            LogUtils.Log("Android Vib");
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        else
        {
#if UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			return true;
#else
        return false;
#endif
    }
}