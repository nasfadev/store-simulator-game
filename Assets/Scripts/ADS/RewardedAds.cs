using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener

{
    [Header("Requires")]
    [SerializeField] DummyAds dummyAds;

    [Header("Configs")]
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    [SerializeField] bool isUsingDummyAds;
    UnityEvent whenRewardedAdsShowComplete;
    UnityEvent whenRewardedAdsFailedToLoad;
    UnityEvent whenRewardedAdsShowFailure;
    bool isLoadAds;
    string _adUnitId = null; // This will remain null for unsupported platforms 

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd(RewardedAdsConfigs configs)
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        if (isLoadAds)
        {
            if (isUsingDummyAds)
            {
                dummyAds.rewardEvent = configs.whenRewardedAdsShowComplete;
                dummyAds.Run();
            }
            Debug.Log("ads gagal");
            return;
        }
        isLoadAds = true;
        whenRewardedAdsShowComplete = configs.whenRewardedAdsShowComplete;
        whenRewardedAdsFailedToLoad = configs.whenRewardedAdsFailedToLoad;
        whenRewardedAdsShowFailure = configs.whenRewardedAdsShowFailure;
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        ShowAd();
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {

        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            whenRewardedAdsShowComplete?.Invoke();
            Debug.Log("Unity Ads Rewarded Ad Completed");
            isLoadAds = false;
            // Grant a reward.
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        whenRewardedAdsFailedToLoad?.Invoke();
        isLoadAds = false;

        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        whenRewardedAdsShowFailure?.Invoke();
        // Use the error details to determine whether to try to load another ad.
        isLoadAds = false;

    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:

    }
}