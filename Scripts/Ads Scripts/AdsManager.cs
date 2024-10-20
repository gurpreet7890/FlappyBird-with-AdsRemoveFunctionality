using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;
using System.Collections;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener
{
    // UI Elements for Ad Manager
    public Button watchAdButton;       // Button to simulate watching ads
    public Button noThanksButton;      // Button to dismiss the ad offer
    public Button goAdFreeButton;      // Button for Go Ad-Free
    public Button continueButton;      // Button for Continue (after ad or going ad-free)
    public Slider adProgressBar;       // Progress bar to show ads watched (out of 3)
    public TMP_Text confirmationText;  // TextMeshPro text element for confirmation messages
    public TMP_Text timerText;         // TextMeshPro text element to display countdown timer
    public GameObject goAdFreeScreen;  // Reference to the Go Ad-Free screen

    // Ad and game management variables
    private int adsWatched = 0;        // Counter for ads watched
    public bool isAdFreePeriod = false; // Flag to check if in ad-free period
    private float remainingAdFreeTime = 0; // Timer for ad-free period
    private int timesWatched;          // Counter for how many times ads have been watched

    // Unity Ads variables
    [SerializeField] private string _androidGameId; // Game ID for Android platform
    [SerializeField] private string _iOSGameId;     // Game ID for iOS platform
    [SerializeField] private bool _testMode = true; // Test mode flag for ads
    private string _gameId;                         // Game ID for the current platform

    // Unity Ads interstitial ad reference
    [SerializeField] private InterstitialAdExample interstitialAds;

    // Called when the script instance is being loaded
    void Awake()
    {
        InitializeAds(); // Initialize Unity Ads during the start of the game
    }

    // Called before the first frame update
    void Start()
    {
        // Set up the UI and variables for the first time
        adProgressBar.maxValue = 3; // Set the max value for the progress bar to 3 (number of ads to watch)
        adProgressBar.value = adsWatched; // Set the current value of the progress bar
        confirmationText.text = ""; // Clear the confirmation text
        timerText.text = "";        // Clear the timer text

        // Load remaining ad-free time from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey("RemainingAdFreeTime"))
        {
            remainingAdFreeTime = PlayerPrefs.GetFloat("RemainingAdFreeTime");
        }

        // If there is remaining ad-free time, continue the ad-free period without resetting the timer
        if (remainingAdFreeTime > 0)
        {
            StartAdFreePeriod(false); // Continue from the saved time without resetting
        }

        // Set up button listeners
        watchAdButton.onClick.AddListener(OnWatchAdButtonClicked);
        noThanksButton.onClick.AddListener(OnNoThanksButtonClicked);
    }

    // Called once per frame
    void Update()
    {
        // If the player is in an ad-free period, update the timer
        if (isAdFreePeriod)
        {
            remainingAdFreeTime -= Time.deltaTime; // Decrease the remaining time each frame

            // Save the remaining time persistently in PlayerPrefs
            PlayerPrefs.SetFloat("RemainingAdFreeTime", remainingAdFreeTime);
            PlayerPrefs.Save();

            // Convert remaining time to minutes and seconds
            int minutes = Mathf.FloorToInt(remainingAdFreeTime / 60F);
            int seconds = Mathf.FloorToInt(remainingAdFreeTime % 60F);
            timerText.text = $"Ad-Free Time: {minutes:00}:{seconds:00}";

            // If the ad-free period has ended, trigger the end function
            if (remainingAdFreeTime <= 0)
            {
                EndAdFreePeriod();
            }
        }
    }

    // Initialize Unity Ads
    public void InitializeAds()
    {
        // Set the appropriate Game ID based on platform
#if UNITY_IOS
        _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
        _gameId = _androidGameId; // For testing in the Unity Editor
#endif

        // Check if Unity Ads is not initialized and is supported, then initialize it
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    // Callback for successful Unity Ads initialization
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    // Callback for Unity Ads initialization failure
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // Triggered when the watch ad button is clicked
    void OnWatchAdButtonClicked()
    {
        // If not in an ad-free period
        if (!isAdFreePeriod)
        {
            // Allow the player to watch ads up to 3 times
            if (timesWatched <= 3)
            {
                timesWatched++;
                Debug.Log(timesWatched);
                interstitialAds.ShowAd(); // Show the Unity interstitial ad
                StartCoroutine(SimulateAdWatching()); // Simulate the ad-watching experience
            }
            else
            {
                goAdFreeScreen.gameObject.SetActive(false); // Hide the Go Ad-Free screen after 3 ads
            }
        }
    }

    // Triggered when the No Thanks button is clicked
    void OnNoThanksButtonClicked()
    {
        confirmationText.text = "Ad offer dismissed."; // Show confirmation for dismissing the ad offer
        this.gameObject.SetActive(false); // Hide the current ad UI
        goAdFreeScreen.gameObject.SetActive(false); // Hide the Go Ad-Free screen
    }

    // Simulate the ad-watching experience
    IEnumerator SimulateAdWatching()
    {
        // Disable the watch ad and no thanks buttons while simulating
        watchAdButton.interactable = false;
        noThanksButton.interactable = false;

        // Wait for 5 seconds to simulate the ad duration
        yield return new WaitForSeconds(5);

        // Re-enable the buttons after ad simulation
        watchAdButton.interactable = true;
        noThanksButton.interactable = true;
        
        adsWatched++; // Increment the number of ads watched
        adProgressBar.value = adsWatched; // Update the progress bar

        // If the player has watched 3 ads, start the ad-free period
        if (adsWatched >= 3)
        {
            StartAdFreePeriod(true); // Start a new ad-free period
        }
        else
        {
            confirmationText.text = $"Ad {adsWatched}/3 watched!"; // Show the number of ads watched
        }
    }

    // Start or continue the ad-free period
    void StartAdFreePeriod(bool resetTime)
    {
        isAdFreePeriod = true; // Set the flag to indicate ad-free period

        // If it's a new ad-free period, reset the timer to 30 minutes
        if (resetTime)
        {
            remainingAdFreeTime = 30 * 60; // 30 minutes
        }

        confirmationText.text = "Ad-free period started!"; // Show confirmation

        // Disable and hide UI elements during the ad-free period
        watchAdButton.interactable = false;
        noThanksButton.interactable = false;
        goAdFreeScreen.SetActive(false);   // Hide Go Ad-Free screen
        goAdFreeButton.gameObject.SetActive(false);  // Hide Go Ad-Free button
        continueButton.gameObject.SetActive(false);  // Hide Continue button

        // Save the remaining ad-free time in PlayerPrefs
        PlayerPrefs.SetFloat("RemainingAdFreeTime", remainingAdFreeTime);
        PlayerPrefs.Save();
    }

    // End the ad-free period
    void EndAdFreePeriod()
    {
        isAdFreePeriod = false; // Reset the ad-free period flag
        confirmationText.text = "Ad-free period has ended. Ads are back!"; // Show confirmation

        // Re-enable and show UI elements after the ad-free period ends
        watchAdButton.interactable = true;
        noThanksButton.interactable = true;
        goAdFreeScreen.SetActive(true);  // Show the Go Ad-Free screen
        goAdFreeButton.gameObject.SetActive(true);   // Show the Go Ad-Free button
        continueButton.gameObject.SetActive(true);   // Show the Continue button

        // Remove the saved ad-free time and reset
        PlayerPrefs.DeleteKey("RemainingAdFreeTime");
    }
}
