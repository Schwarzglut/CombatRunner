﻿using UnityEngine;
using System.Collections;
using System;

public class PlatformsMovement : MonoBehaviour {
    private GameObject platforms;
    private GameObject[] listOfPlatforms;
    // An array holding the positions of the platforms before they move
    private Vector3[] platformsOrgPosition;

    // Platform generation variables
    bool isFirstPlatformActive;
    int newPlatformStartPoint;

    MovementScript ms;
    // Tile type variables
    private int normalTileCount;
    private int encampmentTileCount;
    private int platformType;
    private int encampmentChance;
    private int normalChance;
    private int maxEncampmentCount;

    // Lerping variables
    private float lerpTime;
    private float rateOfLerp;
    private float disBeforeComplete;
    private bool isLerping;
    private Vector3 newPosition;
    private Vector3 orgPosition;
    private Vector3 leftPosition, centerPosition, rightPosition;
    // Use this for initialization
    void Start () {
        // Linking the scripts
        ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        // Grab the platforms and assign them to the array of game objects, listOfPlatforms
        platforms = GameObject.FindGameObjectWithTag("platforms");
        // Populate and initialize the array of platforms
        listOfPlatforms = new GameObject[6];
        platformsOrgPosition = new Vector3[6];
        // Fill the listOfPlatforms array with the children of platforms
        PopulatePlatforms();
        // Choosing platforms variables
        isFirstPlatformActive = false;
        newPlatformStartPoint = 0;
        // Platform type variable initialization
        normalTileCount = 0;
        encampmentTileCount = 0;
        platformType = 0;
        encampmentChance = 0;
        normalChance = 90;
        // Chance this variable for difficulty
        maxEncampmentCount = 3;
        // Lerp variable initialization
        lerpTime = 0;
        rateOfLerp = 1f;
        disBeforeComplete = 0.1f;
        isLerping = false;
        newPosition = new Vector3(0,0,0);
        orgPosition = listOfPlatforms[0].transform.position;
        leftPosition = new Vector3(listOfPlatforms[0].transform.position.x + 4, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        centerPosition = new Vector3(listOfPlatforms[0].transform.position.x, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        rightPosition = new Vector3(listOfPlatforms[0].transform.position.x - 4, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        // GENERATE PLATFORMS
        // Call function to start making the path
        GeneratePlatforms();

        // Subscribe to event
        Subscribe(ms);
    }
    void Update()
    {
        Movement();
    }
    // USER DEFINED FUNCTIONS
    void PopulatePlatforms()
    {
        for (int i =0; i < listOfPlatforms.Length; i++)
        {
            listOfPlatforms[i] = platforms.transform.GetChild(i).gameObject;
            platformsOrgPosition[i] = listOfPlatforms[i].transform.position;
            listOfPlatforms[i].gameObject.SetActive(false);
        }
    }
    // Generating platforms, making them visible or invisible
    void GeneratePlatforms()
    {
        // INDEPENDENT
        if (!isFirstPlatformActive)
        {
            listOfPlatforms[1].gameObject.SetActive(true);
            GeneratePlatforms(1, 0);
            isFirstPlatformActive = true;
        }
    }
    // Generate the platform locations
    void GeneratePlatforms(int newPlatformNumber, int type)
    {
        // REPLACE RECURSION WITH FOR LOOP
        for (int  i = 0; i < listOfPlatforms.Length; i++)
        {
            // 0 == right, 1 == center, 2 == left.
            Vector3 newPlatformPosition = new Vector3(0,0,0);
            if (newPlatformNumber == 0)
            {
                newPlatformNumber = Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
                newPlatformPosition = new Vector3(leftPosition.x, leftPosition.y, listOfPlatforms[i].gameObject.transform.position.z); ;
            }
            else if (newPlatformNumber == 1)
            {
                newPlatformNumber = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
                newPlatformPosition = new Vector3(centerPosition.x, centerPosition.y, listOfPlatforms[i].gameObject.transform.position.z); ;
            }
            else if (newPlatformNumber == 2)
            {
                newPlatformNumber = Mathf.RoundToInt(UnityEngine.Random.Range(1, 3));
                newPlatformPosition = new Vector3(rightPosition.x, rightPosition.y, listOfPlatforms[i].gameObject.transform.position.z); ;
            }
            // Set the choosen next platform to active.
            listOfPlatforms[i].gameObject.SetActive(true);
            listOfPlatforms[i].gameObject.transform.localPosition = newPlatformPosition;
            // Set the choosen next platform to the generated platform type.
            listOfPlatforms[i].transform.GetChild(type).gameObject.SetActive(true);
            // If the platform is an encampment disable the coin.
            if (type != 0)
            {
                listOfPlatforms[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            // Set the next type.
            type = ChooseNextTileType(type);
        }
        newPlatformStartPoint = newPlatformNumber;
    }
    // A function that saves the position that the next row will be
    void GeneratePlatformLoop(GameObject go, int newPlatformNumber, int position)
    {
        // nextRowPlatform is a variable that holds a value between 0 to 2 for the next rows platform.
        int nextRowPlatform = 0;
        Vector3 newPlatformPosition = new Vector3(0, 0, 0);
        if (newPlatformNumber == 0)
        {
            nextRowPlatform = Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
            newPlatformPosition = new Vector3(leftPosition.x, leftPosition.y, listOfPlatforms[position].gameObject.transform.position.z);
        }
        else if (newPlatformNumber == 1)
        {
            nextRowPlatform = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
            newPlatformPosition = new Vector3(centerPosition.x, centerPosition.y, listOfPlatforms[position].gameObject.transform.position.z);
        }
        else if (newPlatformNumber == 2)
        {
            nextRowPlatform = Mathf.RoundToInt(UnityEngine.Random.Range(1, 3));
            newPlatformPosition = new Vector3(rightPosition.x, rightPosition.y, listOfPlatforms[position].gameObject.transform.position.z);
        }
        // Set the choosen next platform to active.
        go.SetActive(true);
        // Give the gameobject its new position.
        go.transform.localPosition = newPlatformPosition;
        // Set the choosen next platform to the generated platform type.
        go.transform.GetChild(ChooseNextTileType(platformType)).gameObject.SetActive(true);
        // Set the new starting point for next row generation.
        newPlatformStartPoint = nextRowPlatform;
    }
    // Function that reset the positions of object and score and basically starts the level again
    // NOTE:NOTE:NOTE: Possibly update this, as when player dies there will be a shop screen, add something that pauses the game
    // UPDATE
    void ResetPlatforms() 
    {
        // Set the platforms back to the stored original positions.
        for (int i = 0; i < listOfPlatforms.Length; i++)
        {
            // Loop through the children and disable them.
            for (int j = 0; j < 4; j++)
            {
                listOfPlatforms[i].transform.GetChild(j).gameObject.SetActive(false);
            }
            listOfPlatforms[i].transform.position = platformsOrgPosition[i];
            listOfPlatforms[i].gameObject.SetActive(false);
        }
        // Choosing platforms variables.
        isFirstPlatformActive = false;
        newPlatformStartPoint = 0;
        // Reseting it so the first tile will be normal
        platformType = 0;
        // Generating the platforms again.
        GeneratePlatforms();
    }
    // A function to reset the children of an object
    void ResetChildren(GameObject go)
    {
        // Loop through the children and disable them.
        for (int i = 0; i < 4; i++)
        {
            go.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // A function to decide if the next platform will be encampment or normal
    // TODO: LOTS OF REPEATED CODE IN THIS FUNCTION!!!
    int ChooseNextTileType(int currentType)
    {
        // Chance this based on a difficultly level of some kind.
        if (encampmentTileCount >= 3)
        {
            platformType = 0;
            encampmentTileCount = 0;
            normalChance += 5;
            encampmentChance -= 2;
            return platformType;
        }
        // If the last platform was not normal then >
        if (currentType != 0)
        {
            // > Geneate random number and test if the normal chance is high enough to set it to normal.
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0, 100)) < normalChance)
            {
                // If the number was below the normalChance number set type to normal.
                platformType = 0;
                // Change the chances depending on what you hit.
                normalChance -= 5;
                encampmentChance += 2;
                // Reset the encampment tile tracker.
                encampmentTileCount = 0;
                // Increase the normal tile tracker.
                normalTileCount++;
            }
            else
            {
                // Generate one of the 3 encampment types.
                platformType = Mathf.RoundToInt(UnityEngine.Random.Range(1,4));
                encampmentChance -= 2;
                normalChance += 5;
                // Increase the encampment tile tracker.
                encampmentTileCount++;
                // Reset the normal tile tracker.
                normalTileCount = 0;
            }
        }
        else if (currentType == 0)
        {
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0,100)) < encampmentChance)
            {
                // Generate one of the 3 encampment types.
                platformType = Mathf.RoundToInt(UnityEngine.Random.Range(1,4));
                encampmentChance -= 2;
                normalChance += 5;
                // Increase the encampment tile tracker.
                encampmentTileCount++;
                // Reset the normal tile tracker.
                normalTileCount = 0;
            }
            else
            {
                // If the number was below the normalChance number set type to normal.
                platformType = 0;
                // Change the chances depending on what you hit.
                encampmentChance += 2;
                normalChance -= 5;
                // Reset the encampment tile tracker.
                encampmentTileCount = 0;
                // Increase the normal tile tracker.
                normalTileCount++;
            }
        }
        //Debug.Log("Normal Chance: " + normalChance + "Encampment Chance: " + encampmentChance);
        return platformType;
    }
    void Movement()
    {
        // Jump right and forward
        if (Input.GetKeyDown(KeyCode.D) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z - 4);
            StartCoroutine(LerpTowards(newPosition, 1));
        }
        // Jump left and forward
        else if (Input.GetKeyDown(KeyCode.A))
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z - 4);
            StartCoroutine(LerpTowards(newPosition, 2));
        }
        // Jump forward
        else if (Input.GetKeyDown(KeyCode.W))
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 4);
            StartCoroutine(LerpTowards(newPosition, 0));
        }
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // A function to move the plaforms based on input from MovementScript
    // COROUTINES
    public IEnumerator LerpTowards(Vector3 towardsPosition, int direction)
    {
        // Directions: 0 == FORWARD, 1 == RIGHT FORWARD, 2 == LEFT FORWARD
        while (isLerping)
        {
            lerpTime += Time.deltaTime * rateOfLerp;
            transform.position = Vector3.Lerp(transform.position, towardsPosition, lerpTime);
            if ((transform.position.z <= towardsPosition.z + disBeforeComplete && direction == 0) ||
                (transform.position.x <= towardsPosition.x + disBeforeComplete && transform.position.z <= towardsPosition.z + disBeforeComplete && direction == 2) ||
                (transform.position.x >= towardsPosition.x - disBeforeComplete && transform.position.z <= towardsPosition.z + disBeforeComplete && direction == 1))
            {
                // If the player is close enough to the final destination set the position.
                // NOTE: Fire a raycast to check if the player position is over a platform or not.
                transform.position = towardsPosition;
                ms.CheckPlatformLanded();
                lerpTime = 0;
                isLerping = false;
                // Called after the position is set to speed up movement.
                MoveLastRow();
            }
            yield return new WaitForSeconds(0);
        }
    }
    // Move the 3 platforms behind the player to the front
    public void MoveLastRow()
    {
        Debug.Log("Calling the function");
        GameObject lastPlatform = null;
        Vector3 playerPosition = new Vector3(0,1,0);
        int position = 0;
        for (int i = 0; i < listOfPlatforms.Length; i++)
        {
            /*// UPDATE MOVEMENT SO THAT THE PLATFORMS ALL MOVE INSTEAD OF THE PARENT OBJECT
            if (listOfPlatforms[i].gameObject.transform.localPosition.z >= playerPosition.z)
            {
                Debug.Log("Entered the loop");
                listOfPlatforms[i].gameObject.SetActive(false);
                listOfPlatforms[i].gameObject.transform.position = new Vector3(listOfPlatforms[i].gameObject.transform.position.x,
                                                                             listOfPlatforms[i].gameObject.transform.position.y,
                                                                             listOfPlatforms[i].gameObject.transform.localPosition.z + 24);
                lastPlatform = listOfPlatforms[i].gameObject;
                ResetChildren(lastPlatform);
                position = i;
                break;
            }*/
        }
        if (lastPlatform != null)
        {
            GeneratePlatformLoop(lastPlatform, newPlatformStartPoint, position);
        }
    }
    // SUBSCRIPTIONS
    // Subscribe to reset event
    public void Subscribe(MovementScript ms)
    {
        ms.Reset += new MovementScript.ResetHandler(ResetPlatforms);
    }
    // Unsubscribe (unsubscribe if listener outlives event caller to break link for garbage collection)
    public void Unsubscribe(MovementScript ms)
    {
        ms.Reset -= new MovementScript.ResetHandler(ResetPlatforms);
    }
}