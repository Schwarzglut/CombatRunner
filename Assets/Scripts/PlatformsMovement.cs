using UnityEngine;
using System.Collections;
using System;

public class PlatformsMovement : MonoBehaviour {
    private float counter;
    private GameObject listOfPlatforms;
    private GameObject[] platforms;
    // An array holding the positions of the platforms before they move
    private Vector3[] platformsOrgPosition;
    // Platform generation variables
    bool isFirstPlatformActive;
    int newPlatformStartPoint;
    // Colour platform variables
    Color platformColour;
    MovementScript ms;
    // Tile type variables
    private int normalTileCount;
    private int encampmentTileCount;
    private int platformType;
    private int encampmentChance;
    private int normalChance;
    // Use this for initialization
    void Start () {
        // Linking the scripts
        MovementScript ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        // Grab the platforms and assign them to the array of game objects, listOfPlatforms
        listOfPlatforms = GameObject.FindGameObjectWithTag("platforms");
        // Populate and initialize the array of platforms
        platforms = new GameObject[18];
        platformsOrgPosition = new Vector3[18];
        populatePlatforms();
        counter = 0; 
        // Choosing platforms variables
        isFirstPlatformActive = false;
        newPlatformStartPoint = 0;
        // Colour platform variable initialization
        platformColour = new Color(0,0,0,0);
        // Platform type variable initialization
        normalTileCount = 0;
        encampmentTileCount = 0;
        platformType = 0;
        encampmentChance = 0;
        normalChance = 0;
        // GENERATE PLATFORMS
        // Call function to start making the path
        GeneratePlatforms();

        // Subscribe to event
        Subscribe(ms);
    }
    // USER DEFINED FUNCTIONS
    // Keeping variables indepedent from coroutines with a function to set counter
    void setCounter(float newValue)
    {
        counter = newValue;
    }
    void populatePlatforms()
    {
        for (int i =0; i < 18; i++)
        {
            platforms[i] = listOfPlatforms.transform.GetChild(i).gameObject;
            platformsOrgPosition[i] = platforms[i].transform.position;
            platforms[i].gameObject.SetActive(false);
        }
    }
    // Generating platforms, making them visible or invisible
    void GeneratePlatforms()
    {
        // INDEPENDENT
        if (!isFirstPlatformActive)
        {
            platforms[1].gameObject.SetActive(true);
            GeneratePlatforms(1);
            isFirstPlatformActive = true;
        }
    }
    // Generate the platform locations
    void GeneratePlatforms(int newPlatformNumber)
    {
        // Generate the random number for the platform type to be made.
        int platformType = Mathf.RoundToInt(UnityEngine.Random.Range(0,4));
        // PROBLEM: Less than 15 only works for the first round of gereration,
        // Changes need for when old platforms must loop back around.
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
            platforms[newPlatformNumber].transform.GetChild(platformType).gameObject.SetActive(true);
            GeneratePlatforms(newPlatformNumber);
        }
        else
        {
            newPlatformStartPoint = newPlatformNumber - 15;
        }
    }
    // A function that saves the position that the next row will be
    void GeneratePlatformLoop(GameObject[] goArray, int newPlatformNumber)
    {
        // Generate the random number for the platform type to be made.
        int platformType = Mathf.RoundToInt(UnityEngine.Random.Range(0, 4));
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
        goArray[temp].transform.GetChild(platformType).gameObject.SetActive(true);
        newPlatformStartPoint = temp;
    }
    // POSSIBLY IGNORE THIS WHOLE THING <<<<<<<
    // A function to set the colour of the platform
    void GeneratePlatformColour(int platform)
    {
        // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
        platforms[platform].GetComponent<Renderer>().material.SetColor(0, Color.red);
    }
    // >>>>>>>
    void setLastRowActive(GameObject go, bool isActive)
    {
        // Loop through the children of the gameobect and set them to false.
        for (int i = 0; i < 4; i++)
        {
            go.transform.GetChild(i).gameObject.SetActive(isActive);
        }
        go.SetActive(isActive);
    }
    void ResetPlatforms()
    {
        // Set the platforms back to the stored original positions.
        for (int i = 0; i < platforms.Length; i++)
        {
            // Loop through the children and disable them.
            for (int j = 0; j < 4; j++)
            {
                platforms[i].transform.GetChild(j).gameObject.SetActive(false);
            }
            platforms[i].transform.position = platformsOrgPosition[i];
            platforms[i].gameObject.SetActive(false);
        }
        // Choosing platforms variables.
        isFirstPlatformActive = false;
        newPlatformStartPoint = 0;
        // Generating the platforms again.
        GeneratePlatforms();
    }
    // A function to decide whether a tile will spawn as normal or encampment
    int GenerateTileType()
    {
        if (normalTileCount <= 4)
        {
            platformType = 0;
            normalTileCount++;
        }
        else if (normalTileCount > 4)
        {
            platformType = Mathf.RoundToInt(UnityEngine.Random.Range(1,4));
            normalChance += 10;
        }
        return platformType;
    }
    // A function to decide if the next platform will be encampment or normal
    int ChooseNextTileType(int currentType)
    {
        // If the last platform was not normal then >
        if (currentType != 0)
        {
            // > Geneate random number and test if the normal chance is high enough to set it to normal.
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0, 100)) < normalChance)
            {
                // If you hit the normal chance range then decrease normal chance and increase encampment chance.
                platformType = 0;
                normalChance -= 5;
                encampmentChance += 5;
            }
            else
            {
                // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 11111
                platformType = Mathf.RoundToInt(UnityEngine.Random.Range(1,4));
                encampmentChance -= 5;
            }
        }
        else if (currentType == 0)
        {
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0,100)) < encampmentChance)
            {
                platformType = Mathf.RoundToInt(UnityEngine.Random.Range(1,4));
                encampmentChance -= 5;
                normalChance += 5; 
            }
            else
            {
                platformType = 0;
                encampmentChance += 5;
                normalChance -= 5;
            }
        }
        return platformType;
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // Move the 3 platforms behind the player to the front
    public IEnumerator MoveLastRow(Vector3 playerPosition)
    {
        GameObject[] lastRow = new GameObject[3];
        for (int i = 2; i < platforms.Length; i+=3)
        {
            // Finding the platforms behind the player.
            if (platforms[i].gameObject.transform.position.z+1 < playerPosition.z) {
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
        // If the last row as been populated.
        if (lastRow[0] != null)
        {
            GeneratePlatformLoop(lastRow, newPlatformStartPoint);
        }
        yield return new WaitForSeconds(0);
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