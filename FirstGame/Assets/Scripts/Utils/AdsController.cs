using UnityEngine.Events;
using UnityEngine;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using System.Collections;
//using TeraJetClient;

public class AdsController : MonoBehaviour
    //, IUnityAdsListener
{
    public const string TAG = "AdsController: ";

    public float EXTRA_REWARDED_COUNTDOWN;

    public enum AdType
    {
        ADMOB, UNITY, OTHER
    }

    public static string UNITY_ANDROID_GAME_ID = "3895807";
    public static string UNITY_IOS_GAME_ID = "3895806";
    public static string UNITY_EXTRA_REWARD_ID = "ExtraRewarded";
    public static string UNITY_GAMEOVER_REWARD_ID = "GameoverRewarded";
    public static string UNITY_INTERSITIAL_ID = "video";

    public static string ADMOB_ANDROID_REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";
    public static string ADMOB_IOS_REWARDED_ID = "ca-app-pub-3940256099942544/1712485313";

    public static string ADMOB_ANDROID_INTERSITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    public static string ADMOB_IOS_INTERSITIAL_ID = "ca-app-pub-3940256099942544/4411468910";

    //private InterstitialAd interstitial;
    //public RewardedAd extraDiamondRewardedAd;
    //public RewardedAd gameOverRewardedAd;

    public System.Action OnExtraDiamondRewardedAdLoaded;
    public System.Action OnGameoverRewardedAdLoaded;
    public System.Action OnExtraDiamondRewardedAdDone;
    public System.Action OnGameoverRewardedAdDone;

    public AdType adType;

    public bool testMode;

    [SerializeField] float lastRewardLoaded;

    #region Singleton

    public static AdsController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //TeraJetClient.TeraJetClient.OnAdLoaded += OnAdRequestReturned;
        //TeraJetClient.TeraJetClient.GetAdsConfig();
    }
    #endregion

    //    private void OnAdRequestReturned()
    //    {
    //        initAdsData();
    //    }

    //    public void initAdsData()
    //    {
    //        initUnityAds();
    //        extraDiamondRewardedAd = RequestRewardedAds();
    //        gameOverRewardedAd = RequestRewardedAds();
    //        RequestInterstitial();
    //    }

    //    #region ADMOB_REWARDED_ADS
    //    public RewardedAd RequestRewardedAds()
    //    {
    //        if (adType != AdType.ADMOB) return null;

    //        string adUnitId;
    //#if UNITY_ANDROID
    //        adUnitId = PlayerPrefs.GetString(PlayerPrefsConfig.ADMOB_REWARD_AD_ID_KEY, ADMOB_ANDROID_REWARDED_ID);
    //#elif UNITY_IPHONE
    //            adUnitId = PlayerPrefs.GetString(PlayerPrefsConfig.ADMOB_REWARD_AD_ID_KEY, ADMOB_IOS_REWARDED_ID);
    //#else
    //            adUnitId = ADMOB_ANDROID_REWARDED_ID;
    //#endif

    //        RewardedAd rewardedAd = new RewardedAd(adUnitId);

    //        // Called when an ad request has successfully loaded.
    //        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    //        // Called when an ad request failed to load.
    //        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    //        // Called when an ad is shown.
    //        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    //        // Called when an ad request failed to show.
    //        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
    //        // Called when the user should be rewarded for interacting with the ad.
    //        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //        // Called when the ad is closed.
    //        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();
    //        // Load the rewarded ad with the request.
    //        rewardedAd.LoadAd(request);

    //        return rewardedAd;
    //    }

    //    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    //    {
    //        string rewardedType = sender.ToString();
    //        lastRewardLoaded = Time.time;

    //        if (System.Object.ReferenceEquals((RewardedAd)sender, extraDiamondRewardedAd))
    //        {
    //            rewardedType = " Extra";
    //            if (OnExtraDiamondRewardedAdLoaded != null)
    //            {
    //                OnExtraDiamondRewardedAdLoaded();
    //            }
    //        }
    //        else if (System.Object.ReferenceEquals((RewardedAd)sender, gameOverRewardedAd))
    //        {
    //            rewardedType = " Gameover";
    //            if (OnGameoverRewardedAdLoaded != null)
    //            {
    //                OnGameoverRewardedAdLoaded();
    //            }
    //        }

    //        MonoBehaviour.print(TAG + "HandleRewardedAdLoaded event received" + rewardedType);
    //    }

    //    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    //    {
    //        MonoBehaviour.print(TAG +
    //            "HandleRewardedAdFailedToLoad event received with message: "
    //                             + args.Message);
    //    }

    //    public void HandleRewardedAdOpening(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleRewardedAdOpening event received");
    //    }

    //    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    //    {
    //        MonoBehaviour.print(TAG +
    //            "HandleRewardedAdFailedToShow event received with message: "
    //                             + args.Message);
    //    }

    //    public void HandleRewardedAdClosed(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleRewardedAdClosed event received");
    //    }

    //    public void HandleUserEarnedReward(object sender, Reward args)
    //    {
    //        string type = args.Type;
    //        double amount = args.Amount;
    //        string rewardedType = sender.ToString();
    //        if (System.Object.ReferenceEquals((RewardedAd)sender, extraDiamondRewardedAd))
    //        {
    //            extraDiamondRewardedAd = RequestRewardedAds();

    //            rewardedType = " Extra";
    //            if (OnExtraDiamondRewardedAdDone != null)
    //            {
    //                OnExtraDiamondRewardedAdDone();
    //            }
    //        }
    //        else if (System.Object.ReferenceEquals((RewardedAd)sender, gameOverRewardedAd))
    //        {
    //            rewardedType = " Gameover";
    //            gameOverRewardedAd = RequestRewardedAds();
    //            if (OnGameoverRewardedAdDone != null)
    //            {
    //                OnGameoverRewardedAdDone();
    //            }
    //        }

    //        MonoBehaviour.print(TAG +
    //            "HandleRewardedAdRewarded event received for "
    //                        + amount.ToString() + " " + type + rewardedType);
    //    }
    //    #endregion 

    //    #region ADMOB_INTERSITIAL_ADS

    //    public void RequestInterstitial()
    //    {
    //        if (adType != AdType.ADMOB) return;
    //#if UNITY_ANDROID
    //        string adUnitId = PlayerPrefs.GetString(PlayerPrefsConfig.ADMOB_INTERSITIAL_AD_ID_KEY, ADMOB_ANDROID_INTERSITIAL_ID);
    //#elif UNITY_IPHONE
    //        string adUnitId = PlayerPrefs.GetString(PlayerPrefsConfig.ADMOB_INTERSITIAL_AD_ID_KEY, ADMOB_IOS_REWARDED_ID);;
    //#else
    //        string adUnitId = "unexpected_platform";
    //#endif

    //        // Initialize an InterstitialAd.
    //        this.interstitial = new InterstitialAd(adUnitId);

    //        // Called when an ad request has successfully loaded.
    //        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
    //        // Called when an ad request failed to load.
    //        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
    //        // Called when an ad is shown.
    //        this.interstitial.OnAdOpening += HandleOnAdOpened;
    //        // Called when the ad is closed.
    //        this.interstitial.OnAdClosed += HandleOnAdClosed;
    //        // Called when the ad click caused the user to leave the application.
    //        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();
    //        // Load the interstitial with the request.
    //        this.interstitial.LoadAd(request);
    //    }

    //    public void HandleOnAdLoaded(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleAdLoaded event received");
    //    }

    //    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleFailedToReceiveAd event received with message: "
    //                            + args.Message);
    //    }

    //    public void HandleOnAdOpened(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleAdOpened event received");
    //    }

    //    public void HandleOnAdClosed(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleAdClosed event received");
    //        RequestInterstitial();
    //    }

    //    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    //    {
    //        MonoBehaviour.print(TAG + "HandleAdLeavingApplication event received");
    //    }

    //    #endregion INTERSITIAL_ADS

    //    #region UNITY_ADS
    //    public string unityGameId;
    //    public void initUnityAds()
    //    {
    //        if (adType != AdType.UNITY) return;

    //#if UNITY_ANDROID
    //        unityGameId = PlayerPrefs.GetString(PlayerPrefsConfig.UNITY_ANDROID_APP_ID_KEY, UNITY_ANDROID_GAME_ID);
    //#elif UNITY_IPHONE
    //        unityGameId = PlayerPrefs.GetString(PlayerPrefsConfig.UNITY_ANDROID_APP_ID_KEY, UNITY_IOS_GAME_ID);
    //#else
    //        unityGameId = "unexpected_platform";
    //#endif

    //        Advertisement.AddListener(this);
    //        Advertisement.Initialize(unityGameId, testMode);
    //    }

    //    public void OnUnityAdsReady(string placementId)
    //    {
    //        lastRewardLoaded = Time.time;
    //        MonoBehaviour.print(TAG + " The ad is ready " + placementId);

    //        // throw new NotImplementedException();
    //        if (placementId == UNITY_EXTRA_REWARD_ID)
    //            Invoke("OnExtraReward", EXTRA_REWARDED_COUNTDOWN);

    //        if (placementId == UNITY_GAMEOVER_REWARD_ID)
    //            if (OnGameoverRewardedAdLoaded != null)
    //                OnGameoverRewardedAdLoaded();
    //    }

    //    public void OnUnityAdsDidError(string message)
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public void OnUnityAdsDidStart(string placementId)
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    //    {
    //        MonoBehaviour.print(TAG + " The ad finish");
    //        if (showResult == ShowResult.Finished)
    //        {
    //            if (placementId == UNITY_EXTRA_REWARD_ID)
    //                if (OnExtraDiamondRewardedAdDone != null)
    //                    OnExtraDiamondRewardedAdDone();
    //            if (placementId == UNITY_GAMEOVER_REWARD_ID)
    //                if (OnGameoverRewardedAdDone != null)
    //                    OnGameoverRewardedAdDone();
    //        }
    //        else if (showResult == ShowResult.Skipped)
    //        {

    //        }
    //        else if (showResult == ShowResult.Failed)
    //        {
    //            MonoBehaviour.print(TAG + " The ad did not finish due to an error.");
    //        }
    //    }

    //    #endregion

    //    public bool isExtraRewardReady()
    //    {
    //        if (adType == AdType.UNITY) return Advertisement.IsReady(UNITY_EXTRA_REWARD_ID);
    //        if (adType == AdType.ADMOB) return extraDiamondRewardedAd != null && extraDiamondRewardedAd.IsLoaded();

    //        return false;
    //    }

    //    public bool isGameoverRewardReady()
    //    {
    //        if (adType == AdType.UNITY) return Advertisement.IsReady(UNITY_GAMEOVER_REWARD_ID);
    //        if (adType == AdType.ADMOB) return gameOverRewardedAd != null && gameOverRewardedAd.IsLoaded();

    //        return false;
    //    }

    //    public void ShowInterstitialAd()
    //    {
    //        // Check if UnityAds ready before calling Show method:
    //        if (adType == AdType.ADMOB)
    //        {
    //            if (this.interstitial.IsLoaded())
    //            {
    //                this.interstitial.Show();
    //            }
    //        }
    //        else if (adType == AdType.UNITY)
    //        {
    //            if (Advertisement.IsReady())
    //            {
    //                Advertisement.Show(UNITY_INTERSITIAL_ID);
    //            }
    //            else
    //            {
    //                MonoBehaviour.print(TAG + " Interstitial ad not ready at the moment! Please try again later!");
    //            }
    //        }
    //    }

    //    public void ShowExtraDiamondRewardedAds()
    //    {
    //        if (adType == AdType.UNITY)
    //        {
    //            if (Advertisement.IsReady(UNITY_EXTRA_REWARD_ID))
    //            {
    //                Advertisement.Show(UNITY_EXTRA_REWARD_ID);
    //            }
    //            else
    //            {
    //                MonoBehaviour.print(TAG + " Rewarded video is not ready at the moment! Please try again later!");
    //            }
    //        }
    //        else if (adType == AdType.ADMOB)
    //        {
    //            if (this.extraDiamondRewardedAd.IsLoaded())
    //            {
    //                this.extraDiamondRewardedAd.Show();
    //            }
    //        }
    //    }

    //    public void ShowGameoverRewardedAds()
    //    {
    //        if (adType == AdType.UNITY)
    //        {
    //            if (Advertisement.IsReady(UNITY_GAMEOVER_REWARD_ID))
    //            {
    //                Advertisement.Show(UNITY_GAMEOVER_REWARD_ID);
    //            }
    //            else
    //            {
    //                MonoBehaviour.print(TAG + " Gameover Rewarded video is not ready at the moment! Please try again later!");
    //            }
    //        }
    //        else if (adType == AdType.ADMOB)
    //        {
    //            if (this.gameOverRewardedAd.IsLoaded())
    //            {
    //                this.gameOverRewardedAd.Show();
    //            }
    //        }
    //    }

    //    public void ShowRewardPopup(float delay)
    //    {
    //        StartCoroutine(ShowRewardPop(delay));
    //    }

    //    IEnumerator ShowRewardPop(float delay)
    //    {
    //        yield return new WaitForSeconds(delay);
    //        GameUtils.ShowPopup(GameUtils.PopupType.REWARD);
    //    }

    //    public void OnExtraReward()
    //    {
    //        if (OnExtraDiamondRewardedAdLoaded != null)
    //            OnExtraDiamondRewardedAdLoaded();
    //    }

}
