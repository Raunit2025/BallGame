using UnityEngine;
using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [Header("AdMob Unit IDs")]
    // REPLACE THESE WITH YOUR REAL ADMOB IDS BEFORE PUBLISHING
    // These are Google's Test IDs for testing safely
#if UNITY_ANDROID
    [SerializeField] private string _rewardedUnitId = "ca-app-pub-3940256099942544/5224354917"; 
#elif UNITY_IPHONE
    [SerializeField] private string _rewardedUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    [SerializeField] private string _rewardedUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;
    private Action _onRewardSuccess;

    void Awake()
    {
        // SINGLETON PATTERN
        // This handles your "Two Objects" issue automatically.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If we are in Level 1 and the Main Menu manager exists, 
            // destroy this new duplicate one.
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // --- CRITICAL FIX FOR CRASHING ---
        // This tells AdMob to run callbacks on the main Unity thread.
        // Without this, the app crashes when an ad finishes.
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Initialize the SDK
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob Initialized. Loading first ad...");
            LoadRewardedAd();
        });
    }

    /// <summary>
    /// Loads the Rewarded Ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading AdMob Rewarded Ad...");

        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If error is not null, the load failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load with error: " + error);
                // Optional: Retry after 5 seconds if load failed
                Invoke(nameof(LoadRewardedAd), 5.0f);
                return;
            }

            Debug.Log("Rewarded ad loaded successfully.");
            _rewardedAd = ad;
            
            // Register event handlers
            RegisterEventHandlers(_rewardedAd);
        });
    }

    /// <summary>
    /// Shows the Rewarded Ad and triggers the callback if successful.
    /// Keeps the exact signature needed by your RewardAdButton.cs
    /// </summary>
    /// <param name="onSuccess">Action to perform when user earns reward</param>
    public void ShowRewardedAd(Action onSuccess)
    {
        _onRewardSuccess = onSuccess;

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // The user watched the video!
                Debug.Log($"Reward Earned: {reward.Amount} {reward.Type}");
                
                if (_onRewardSuccess != null)
                {
                    _onRewardSuccess.Invoke();
                    _onRewardSuccess = null; // Reset callback
                }
            });
        }
        else
        {
            Debug.Log("Ad not ready. Trying to reload...");
            LoadRewardedAd();
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is closed (either finished or skipped)
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ad closed. Loading next ad...");
            // Immediately load the next ad so it's ready for the next button press
            LoadRewardedAd();
        };

        // Raised when the ad fails to open
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Ad failed to show: " + error);
            // Try to reload
            LoadRewardedAd();
        };
    }
}