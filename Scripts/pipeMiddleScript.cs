using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    // Reference to LogicScript to manage game logic like scoring
    public LogicScript logic;

    // Start is called before the first frame update
    void Start()
    {
        // Find and store the LogicScript from the game object tagged "Logic"
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // No updates are needed in this script for now
    }

    // Method called when another object enters this trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is on layer 3 (player layer)
        if (collision.gameObject.layer == 3)
        {
            // Add 1 to the player's score via the LogicScript
            logic.addScore(1);
        }
    }
}
