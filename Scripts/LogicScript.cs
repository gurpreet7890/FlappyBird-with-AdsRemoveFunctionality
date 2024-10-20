using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    // Public variables
    public int playerScore;                  // Player's score
    public Text scoreText;                   // UI Text element to display the score
    public GameObject gameOverScreen;        // Game over UI screen
    public GameObject adFreeScreen;          // Screen to show when player goes ad-free

    // References to ad systems
    [SerializeField] private AdsManager adsManager;                 // Handles ad initialization and management
    [SerializeField] private RewardedAdsButton rewardedAdsButton;   // Manages rewarded ads button functionality
    [SerializeField] private InterstitialAdExample interstitialAds;  // Manages interstitial ads

    // Private variables
    private int gameOverCount = 0;           // Tracks how many times game over has occurred

    // Start is called before the first frame update
    void Start()
    {
        // Load gameOverCount from PlayerPrefs (if not set, it defaults to 0)
        gameOverCount = PlayerPrefs.GetInt("gameOverCount", 0);
        Debug.Log("Loaded Game Over Count: " + gameOverCount);

        // Initialize ads and load the rewarded ads when the game starts
        adsManager.InitializeAds();
        rewardedAdsButton.LoadAd();
    }

    // Method to increase the player's score
    public void addScore(int scoreToAdd)
    {
        // Add the specified amount to the player's score
        playerScore += scoreToAdd;
        
        // Update the score UI to reflect the new score
        scoreText.text = playerScore.ToString();
    }

    // Method to restart the game by reloading the current scene
    public void restartGame()
    {
        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Reinitialize ads after restarting the scene
        adsManager.InitializeAds();
        rewardedAdsButton.LoadAd();
    }

    // Method to display the ad-free screen
    public void goAdFree()
    {
        // Activate the screen that shows the player they are ad-free
        adFreeScreen.SetActive(true);
    }

    // Method to go back to showing ads
    public void goBackToAds()
    {
        // Deactivate the ad-free screen to resume showing ads
        adFreeScreen.SetActive(false);
    }

    // Method to handle game over logic
    public void gameOver()
    {
        // Show the game over UI screen
        gameOverScreen.SetActive(true);

        // Increment the gameOverCount and save it to PlayerPrefs for persistence
        gameOverCount++;
        PlayerPrefs.SetInt("gameOverCount", gameOverCount);
        Debug.Log("Game Over Count: " + gameOverCount);

        // Check if the gameOverCount has reached 3, then show an interstitial ad
        if (gameOverCount >= 3)
        {
            // Display the interstitial ad
            interstitialAds.ShowAd();
            Debug.Log("Showing Interstitial Ad");

            // Reset the gameOverCount and save the reset value to PlayerPrefs
            gameOverCount = 0;
            PlayerPrefs.SetInt("gameOverCount", gameOverCount);
        }
    }
}
