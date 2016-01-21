using UnityEngine;
using System.Collections;
using System;

public class PlatformsMovement : MonoBehaviour {
    private float counter, timeToMeet, moveSpeed;
    private GameObject listOfPlatforms;
    private GameObject[] platforms;
    // START of LERP variables
    bool isLerping, isLerpingLeft, isLerpingRight;
    Vector3 newPosition, newPositionSideways;
    float lerpTime, rateOfLerp, lerpTimeRight, lerpTimeLeft;
    // END of LERP variables
    // Platform generation variables
    int randomPlatformNumber, unactivePlatformTracker, platformChosen;
    bool isFirstPlatformActive;
    // Use this for initialization
    void Start () {
        // Grab the platforms and assign them to the array of game objects, listOfPlatforms
        listOfPlatforms = GameObject.FindGameObjectWithTag("platforms");
        // Populate and initialize the array of platforms
        platforms = new GameObject[15];
        populatePlatforms();
        counter = 0; 
        // The time the player has to move to the next platform.
        timeToMeet = 100f;
        // Setting the move speed
        moveSpeed = 4.0f;
        // Lerping variables
        isLerping = false; isLerpingLeft = false; isLerpingRight = false;
        lerpTime = 0.0f;
        lerpTimeRight = 0.0f;
        lerpTimeLeft = 0.0f;
        rateOfLerp = 0.5f;
        // Setting the positions for platform movement
        newPosition = transform.position;
        newPositionSideways = transform.position;
        // Choosing platforms variables
        randomPlatformNumber = -1;
        unactivePlatformTracker = 0;
        isFirstPlatformActive = false;
        platformChosen = 0;
        // GENERATE PLATFORMS
        // Call function to start making the path
        GeneratePlatforms();
    }
	// Update is called once per frame
	void Update () {
        //MovePlatforms();
        //counter++;
        Debug.Log("Platform position: " + platforms[0].transform.position);
    }
    void FixedUpdate()
    {
        // Back up to make sure the lerping variables are reset.
        if (transform.position == getNewPosition())
        {
            isLerpingRight = false; isLerpingLeft = false; isLerping = false;
        }
    }
    // USER DEFINED FUNCTIONS
    // Keeping variables indepedent from coroutines with a function to set counter
    void setCounter(float newValue)
    {
        counter = newValue;
    }
    // Get the new position for use inside coroutines
    Vector3 getNewPosition()
    {
        return new Vector3(newPositionSideways.x, newPositionSideways.y, newPosition.z);
    }
    void populatePlatforms()
    {
        for (int i =0; i < 15; i++)
        {
            platforms[i] = listOfPlatforms.transform.GetChild(i).gameObject;
            platforms[i].gameObject.SetActive(false);
        }
    }
    // Automatically move the platforms backwards after a certain time
    void MovePlatforms()
    {
        if (counter >= timeToMeet && !isLerping)
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 4);
            //StartCoroutine(LerpBackwards());
            isLerping = true;
        }
    }
    // Generating platforms, making them visible or invisible
    void GeneratePlatforms()
    {
        // INDEPENDENT
        if (!isFirstPlatformActive)
        {
            randomPlatformNumber = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
            platforms[randomPlatformNumber].gameObject.SetActive(true);
            //Debug.Log("Holy Shit look at me::::: " + randomPlatformNumber);
            GeneratePlatforms(randomPlatformNumber);
            isFirstPlatformActive = true;
        }
    }
    void GeneratePlatforms(int newPlatformNumber)
    {
        // PROBLEM: Less than 12 only works for the first round of gereration,
        // Changes need for when old platforms must loop back around
        if (newPlatformNumber <= 12)
        {
            if (platforms[newPlatformNumber].gameObject.tag == "left")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(2, 4));
                platforms[newPlatformNumber].gameObject.SetActive(true);
                GeneratePlatforms(newPlatformNumber);
            }
            else if (platforms[newPlatformNumber].gameObject.tag == "right")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(3, 5));
                platforms[newPlatformNumber].gameObject.SetActive(true);
                GeneratePlatforms(newPlatformNumber);
            }
            else if (platforms[newPlatformNumber].gameObject.tag == "center")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(2, 5));
                platforms[newPlatformNumber].gameObject.SetActive(true);
                GeneratePlatforms(newPlatformNumber);
            }
        }
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    public void MovePlatformsOverwrite()
    {
        if (!isLerping && !isLerpingLeft && !isLerpingRight) {
            newPosition = new Vector3(newPositionSideways.x, newPositionSideways.y, transform.position.z - 4);
            //StartCoroutine(LerpBackwards());
            isLerping = true;
        }
    }
    // Move the 3 platforms behind the player to the front
    public void MoveLastRow(Vector3 playerPosition)
    {
        Vector3 newRowPosition = playerPosition;
        ArrayList listOfGameObjects = new ArrayList(platforms);
        for(int i = 0; i < listOfGameObjects.Capacity; i++)
        {
            // LOOK UP HOW TO .GET IN AN ARRAYLIST
            if (listOfGameObjects)
            {
                // Reset the position of the 3 gameobjects behind the player to be at the front
                
            }
        }
    }
}