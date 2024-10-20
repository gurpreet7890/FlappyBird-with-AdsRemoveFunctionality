using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    // Public variables for pipe spawning
    public GameObject pipe;              // Pipe prefab to spawn
    public float spawnRate = 2;          // Time interval between pipe spawns
    private float timer = 0;             // Timer to track spawn intervals
    public float heightOffset = 10;      // Vertical range for random pipe spawn position

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the first pipe when the game starts
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the timer by the time elapsed since the last frame
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;  // Accumulate time
        }
        else
        {
            // Spawn a new pipe and reset the timer once the spawnRate is reached
            spawnPipe();
            timer = 0;
        }
    }

    // Method to spawn a pipe at a random height within the heightOffset range
    void spawnPipe()
    {
        // Calculate the lowest and highest points for pipe spawning
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        // Instantiate the pipe at a random height between lowestPoint and highestPoint
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
