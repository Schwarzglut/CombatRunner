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
    int newPlatformStartPoint;
    // Use this for initialization
    void Start () {
        // Grab the platforms and assign them to the array of game objects, listOfPlatforms
        listOfPlatforms = GameObject.FindGameObjectWithTag("platforms");
        // Populate and initialize the array of platforms
        platforms = new GameObject[18];
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
        newPlatformStartPoint = 0;
        // GENERATE PLATFORMS
        // Call function to start making the path
        GeneratePlatforms();
    }
	// Update is called once per frame
	void Update () {
        //MovePlatforms();
        //counter++;
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
        for (int i =0; i < 18; i++)
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
            GeneratePlatforms(randomPlatformNumber);
            isFirstPlatformActive = true;
        }
    }
    void GeneratePlatforms(int newPlatformNumber)
    {
        // PROBLEM: Less than 15 only works for the first round of gereration,
        // Changes need for when old platforms must loop back around
        if (newPlatformNumber <= 14)
        {
            if (platforms[newPlatformNumber].gameObject.tag == "left")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(2, 4));
            }
            else if (platforms[newPlatformNumber].gameObject.tag == "right")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(3, 5));
            }
            else if (platforms[newPlatformNumber].gameObject.tag == "center")
            {
                newPlatformNumber += Mathf.RoundToInt(UnityEngine.Random.Range(2, 5));
            }
            platforms[newPlatformNumber].gameObject.SetActive(true);
            GeneratePlatforms(newPlatformNumber);
        }
        else
        {
            newPlatformStartPoint = newPlatformNumber - 15;
        }
    }
    void GeneratePlatformLoop(GameObject[] goArray, int newPlatformNumber)
    {
        int temp = 0;
        if (newPlatformNumber == 0)
        {
            temp = Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
        }
        else if (newPlatformNumber == 1)
        {
            temp = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
        }
        else if (newPlatformNumber == 2)
        {
            temp = Mathf.RoundToInt(UnityEngine.Random.Range(1, 3));
        }
        goArray[temp].gameObject.SetActive(true);
        newPlatformStartPoint = temp;
    }
    void setLastRowActive(GameObject go, bool isActive)
    {
        go.SetActive(isActive);
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
    public IEnumerator MoveLastRow(Vector3 playerPosition)
    {
        GameObject[] lastRow = new GameObject[3];
        for(int i = 2; i < platforms.Length; i+=3)
        {
            // Finding the platforms behind the player.
            if (platforms[i].gameObject.transform.position.z < playerPosition.z) {
                // Reset the platform position to be 20 units infront of the player.
                int placer = 0;
                for (int j = i-2; j <= i; j++) {
                    platforms[j].gameObject.transform.position = new Vector3(platforms[j].gameObject.transform.position.x, 
                                                                             platforms[j].gameObject.transform.position.y, 
                                                                             platforms[j].gameObject.transform.position.z + 24);
                    setLastRowActive(platforms[j], false);
                    lastRow[placer] = platforms[j];
                    placer++;
                } 
            }
        }
        GeneratePlatformLoop(lastRow, newPlatformStartPoint);
        yield return new WaitForSeconds(0);
    }
}