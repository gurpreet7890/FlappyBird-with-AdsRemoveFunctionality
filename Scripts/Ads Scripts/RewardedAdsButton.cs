using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button _showAdButton; // Button to show the ad
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android"; // Android Ad Unit ID
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS"; // iOS Ad Unit ID
    private string _adUnitId; // Ad Unit ID based on the platform
    private LogicScript logicScript; // Reference to LogicScript to manage game state
    public AdsManager adsManager; // Reference to AdsManager to access isAdFreePeriod

    void Start()
    {
        // Find the LogicScript component in the scene
        logicScript = FindObjectOfType<LogicScript>();

        // Set the Ad Unit ID based on the platform
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show
        _showAdButton.interactable = false;

        // Load the ad if not in the ad-free period
        if (!adsManager.isAdFreePeriod)
        {
            LoadAd();
        }
        else
        {
            Debug.Log("Ad-Free Period Active: Skipping Rewarded Ad.");
        }
    }

    // Load the rewarded ad
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Callback for when the ad has successfully loaded
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to show the ad when clicked
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for user interaction
            _showAdButton.interactable = true;
        }
    }

    // Show the rewarded ad when the button is clicked
    public void ShowAd()
    {
        // Disable the button to prevent multiple clicks
        _showAdButton.interactable = false;
        // Show the ad
        Advertisement.Show(_adUnitId, this);
    }

    // Callback for when the ad completes showing
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward and resume the game
            //logicScript.ResumeGameAfterReward(); // Call the method in your LogicScript that resumes the game
        }
    }

    // Callback for when an ad fails to load
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");
        // Optionally, retry loading the ad
        LoadAd();
    }

    // Callback for when an ad fails to show
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");
        // Optionally, retry loading the ad
        LoadAd();
    }

    // Callbacks for ad events
    public void OnUnityAdsShowStart(string adUnitId) 
    {
        Debug.Log($"Ad Started Showing: {adUnitId}");
    }

    public void OnUnityAdsShowClick(string adUnitId) 
    {
        Debug.Log($"Ad Clicked: {adUnitId}");
    }

    void OnDestroy()
    {
        // Clean up the button listeners to prevent memory leaks
        _showAdButton.onClick.RemoveAllListeners();
    }
}
