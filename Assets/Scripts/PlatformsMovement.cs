using UnityEngine;
using System.Collections;

public class PlatformsMovement : MonoBehaviour {
    private float counter, timeToMeet, moveSpeed;
    private bool playerMovedForward;
	// Use this for initialization
	void Start () {
        counter = 0; 
        // The time the player has to move to the next platform.
        timeToMeet = 100f;
        // The boolean to check that the player is on the same
        // platform row, otherwise the row moves forward
        playerMovedForward = false;
        // Setting the move speed
        moveSpeed = 4.0f;
    }
	
	// Update is called once per frame
	void Update () {
        MovePlatforms();
        counter++;
        Debug.Log(counter);
	}
    void MovePlatforms()
    {
        if (counter >= timeToMeet && !playerMovedForward)
        {
            transform.Translate(Vector3.back * moveSpeed);
            counter = 0;
        }
    }
}
