using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfigs
{
    #region RATE_CONFIGS

    public static readonly string RATE_PLAY_COUNT_KEY = "rate_play_count_key";
    public static readonly string RATE_DISABLE_KEY = "rate_disable_key";
    public static readonly int PLAY_COUNT_LIMITED = 3;
    public static readonly int MIN_FINISH_POWER = 100;

    #endregion

    public static float TOUCH_HOLD_THRESHHOLD = 0.05f;
    public static string UNLOCK_ITEM_PROCESS_KEY = "UNLOCK_ITEM_PROCESS_KEY";
    public static string PICKUP_KEY_PROCESS_KEY = "PICKUP_KEY_PROCESS_KEY";
}
