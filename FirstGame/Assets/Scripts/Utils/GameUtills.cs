using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public static class GameUtils {
    public static readonly string PLATFORM_ANDROID = "Android";
    public static readonly string PLATFORM_IOS = "iOS";
    public static readonly string PLATFORM_WEB = "Web";

    static float acumTime;
    public static float hold_threshhold = 0.2f;

    #region PLAYERDATA_UTILS
    public static void SavePlayerData(UserData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.terajet";

        FileStream stream = new FileStream(path, FileMode.Create);

        // PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static UserData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.terajet";
        if (File.Exists(path))
        {
            Debug.Log("Saved file found in " + path);

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            UserData data = formatter.Deserialize(stream) as UserData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Saved file not found in " + path + " Generate Zero Data");
            UserData data = new UserData();
            return data;
        }
    }

    #endregion

    #region SYSTEM_UTILS
    public static T[] AddItemToArray<T>(this T[] original, T itemToAdd)
    {
        T[] finalArray = new T[original.Length + 1];
        for (int i = 0; i < original.Length; i++)
        {
            finalArray[i] = original[i];
        }
        finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    public static T[] Reverse<T>(T[] array)
    {
        var result = new T[array.Length];
        int j = 0;
        for (int i = array.Length - 1; i >= 0; i--)
        {
            result[j] = array[i];
            j++;
        }
        return result;
    }
    #endregion

    #region EXCUTE_FUNCTION

    public delegate void FunctionType();

    public static FunctionType excuteFunction { set { excuteFunction = value; ExcuteFunction(value, 0f); } }

    public static void ExcuteFunction(FunctionType function, float time)
    {
        GameManager.Instance.StartCoroutine(ExcuteFunctionIE(function, time));
    }

    static IEnumerator ExcuteFunctionIE(FunctionType function, float time)
    {
        yield return new WaitForSeconds(time);
        function();
    }

    #endregion

    #region REQUEST_PERMISSIONS

    public static void RequestPermission(MonoBehaviour monoBehaviour)
    {

    }

    #endregion

    #region POP_UP_UTILS
    public enum PopupType
    {
        SIMPLE, LACK_OF_RESOURCE, PROGRESS, CONFIRM_PURCHASED, BONUS, REWARD, RATE
    }

    public static void ShowRateDialog()
    {
#if UNITY_ANDROID
        foreach (Canvas item in UnityEngine.Object.FindObjectsOfType(typeof(Canvas)))
        {
            if (item.renderMode == RenderMode.ScreenSpaceOverlay && item.name.StartsWith("UICanvas"))
            {
                GameObject popup = GameObject.Instantiate(Resources.Load("UI/Popup", typeof(GameObject)) as GameObject, item.transform);

                PopupController popupController;
                popupController = popup.GetComponent<PopupController>();
                popupController.type = PopupType.RATE;
                popupController.title = "Rate us";
                popupController.message = "If you enjoy playing this game, please take a moment to rate it. Thanks for your support!";
                return;
            }
        }
#elif UNITY_IOS
        Device.RequestStoreReview();
#endif
        #endregion
    }

    public static GameObject ShowPopup(PopupType popupType, string title, string message, int price, bool isGift)
    {
        GameObject popup = null;
        return popup;
    }

    public static void TouchInput(MonoBehaviour gameobject, string tapInvokeMethod, string holdInvokeMethod, string releaseInvokeMethod)
    {
        if (Input.touchCount > 0)
        {
            acumTime += Input.GetTouch(0).deltaTime;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (acumTime < hold_threshhold)
                {
                    if (!string.IsNullOrEmpty(tapInvokeMethod))
                        gameobject.Invoke(tapInvokeMethod, 0f);
                } else {
                    if (!string.IsNullOrEmpty(releaseInvokeMethod))
                        gameobject.Invoke(releaseInvokeMethod, 0f);
                }
                acumTime = 0;
            }

            if (acumTime >= hold_threshhold)
            {
                //Long tap
                LogUtils.Log("HOLD HOLD");
                if(!string.IsNullOrEmpty(holdInvokeMethod))
                gameobject.Invoke(holdInvokeMethod, 0f);
            }

        }

    }

    
}
