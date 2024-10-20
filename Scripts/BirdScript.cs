using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    // Public variables
    public Rigidbody2D myRigidbody;     // Reference to the bird's Rigidbody2D component
    public float flapStrength;          // Strength of the bird's flap (force applied when space is pressed)
    public LogicScript logic;           // Reference to the LogicScript handling game over and scoring logic
    public bool birdIsAlive = true;     // Bool to track if the bird is alive
    public int gameOverCount = 0;       // Counter to track the number of game overs (potential use for ads)

    // Private variables
    private InterstitialAdExample interstitialAd; // Reference to the ad management script (if any)

    // Start is called before the first frame update
    void Start()
    {
        // Find and store the LogicScript from the game object tagged "Logic"
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the spacebar is pressed and the bird is alive
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            // Apply an upward force to the bird to simulate flapping
            myRigidbody.velocity = Vector2.up * flapStrength;
        }
    }

    // Method triggered when the bird collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ensure gameOver is only called if the bird is alive at the time of collision
        if (birdIsAlive)
        {
            logic.gameOver();  // Trigger the game over sequence in the LogicScript
            birdIsAlive = false;  // Set birdIsAlive to false to prevent multiple gameOver calls
        }
    }
}
