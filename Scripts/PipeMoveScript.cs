using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    // Public variables for movement and destruction
    public float moveSpeed = 5;    // Speed at which the pipe moves to the left
    public float deadZone = -45;   // X-position at which the pipe gets destroyed

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code (if needed) can go here
    }

    // Update is called once per frame
    void Update()
    {
        // Move the pipe to the left at a constant speed, scaled by deltaTime for smooth movement
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        // If the pipe moves past the deadZone, destroy it to free up memory and resources
        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe Deleted!"); // Log the pipe deletion (for debugging purposes)
            Destroy(gameObject);        // Destroy the pipe game object
        }
    }
}
