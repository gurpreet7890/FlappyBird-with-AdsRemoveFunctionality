using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdExample : MonoBehaviour
{
    // Configurable Banner Position and Ad Unit IDs for Android and iOS
    [SerializeField] private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER; // Position of the banner
    [SerializeField] private string _androidAdUnitId = "Banner_Android"; // Android ad unit ID
    [SerializeField] private string _iOSAdUnitId = "Banner_iOS"; // iOS ad unit ID

    private string _adUnitId = null; // Will store the appropriate Ad Unit ID based on the platform

    // Reference to the AdsManager script to check if ad-free period is active
    public AdsManager adsManager;  

    // Called before the first frame update
    void Start()
    {
        // Get the correct Ad Unit ID based on the platform
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Set the banner position on the screen
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Automatically load and show the banner ad if the ad-free period is not active
        if (!adsManager.isAdFreePeriod)
        {
            LoadAndShowBanner();
        }
        else
        {
            Debug.Log("Ad-Free Period Active: Skipping Banner Ad.");
        }
    }

    // Method to load and display the banner ad
    private void LoadAndShowBanner()
    {
        // Set up options to receive callbacks for banner loading events
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,   // Callback when the banner is loaded
            errorCallback = OnBannerError    // Callback when loading the banner fails
        };

        // Load the banner ad using the appropriate Ad Unit ID
        Advertisement.Banner.Load(_adUnitId, options);
    }

    // Callback method for when the banner ad is successfully loaded
    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded successfully");

        // Automatically show the banner if the ad-free period is not active
        if (!adsManager.isAdFreePeriod)
        {
            ShowBannerAd();
        }
    }

    // Callback method for when there is an error loading the banner ad
    private void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally, you could try loading a fallback ad here if desired
    }

    // Method to show the banner ad on the screen
    private void ShowBannerAd()
    {
        // Set up options to handle banner click, hide, and show events
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,   // Callback for when the banner is clicked
            hideCallback = OnBannerHidden,     // Callback for when the banner is hidden
            showCallback = OnBannerShown       // Callback for when the banner is shown
        };

        // Show the loaded banner ad using the options defined
        Advertisement.Banner.Show(_adUnitId, options);
    }

    // Optional callback for when the banner is clicked by the user
    private void OnBannerClicked()
    {
        Debug.Log("Banner clicked by user");
    }

    // Optional callback for when the banner is shown
    private void OnBannerShown()
    {
        Debug.Log("Banner is now visible");
    }

    // Optional callback for when the banner is hidden
    private void OnBannerHidden()
    {
        Debug.Log("Banner is now hidden");
    }

    // Called when the GameObject is destroyed
    void OnDestroy()
    {
        // Hide the banner ad when the object is destroyed to prevent it from remaining visible
        Advertisement.Banner.Hide();
        
        // You can also remove any other event listeners or cleanup resources here if necessary
    }
}
