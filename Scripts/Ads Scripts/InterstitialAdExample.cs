using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdExample : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // Ad Unit IDs for Android and iOS platforms
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android"; // Android Ad Unit ID
    [SerializeField] private string _iOsAdUnitId = "Interstitial_iOS"; // iOS Ad Unit ID
    private string _adUnitId; // Stores the platform-specific Ad Unit ID

    // Reference to AdsManager to check if the ad-free period is active
    public AdsManager adsManager;

    // Tracks if the ad has been successfully loaded
    private bool isAdLoaded = false;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Assign the Ad Unit ID based on the platform
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    // Called when the scene starts
    void Start()
    {
        // Load the ad content when the scene starts
        LoadAd();
    }

    // Method to load the interstitial ad
    public void LoadAd()
    {
        // If the ad-free period is active, skip loading ads
        if (adsManager.isAdFreePeriod)
        {
            Debug.Log("Ad-Free Period Active: Skipping Ad Load.");
            return;
        }

        // Start loading the ad
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Method to show the interstitial ad if it's loaded
    public void ShowAd()
    {
        // Check if the ad-free period is active before showing the ad
        if (adsManager.isAdFreePeriod)
        {
            Debug.Log("Ad-Free Period Active: Skipping Interstitial Ad.");
            return;
        }

        // Check if the ad has been loaded successfully
        if (isAdLoaded)
        {
            Debug.Log("Showing Interstitial Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this); // Show the ad
        }
        else
        {
            Debug.Log("Interstitial ad not ready.");
        }
    }

    // Callback when the ad is successfully loaded
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            Debug.Log($"Ad Unit Loaded: {adUnitId}");
            isAdLoaded = true; // Mark the ad as loaded
        }
    }

    // Callback when there is an error loading the ad
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        isAdLoaded = false; // Mark the ad as not loaded
    }

    // Callback when there is an error showing the ad
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        isAdLoaded = false; // Reset the ad loaded status
        LoadAd(); // Reload the ad for future attempts
    }

    // Callback when the ad starts showing
    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"Ad Started Showing: {adUnitId}");
    }

    // Callback when the ad is clicked
    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"Ad Clicked: {adUnitId}");
    }

    // Callback when the ad completes or is skipped
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("Ad completed successfully.");
            }
            else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            {
                Debug.Log("Ad skipped.");
            }

            // Reload the ad to be ready for the next use
            isAdLoaded = false;
            LoadAd();
        }
    }
}
