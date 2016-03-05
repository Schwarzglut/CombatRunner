using UnityEngine;
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
    // TODO: USE THIS VARIABLE TO STOP TOO MANY ENCAMPMENTS SPAWNING IN A ROW.
    private int maxEncampmentCount;
    private int coinChance;
    private int typeToBe;

    // Lerping variables
    private float lerpTime;
    private float rateOfLerp;
    private float disBeforeComplete;
    private bool isLerping;
    private Vector3 newPosition;
    private Vector3 orgPosition;
    private Vector3 leftPosition, centerPosition, rightPosition;
    private Vector3[] newPlatformPositions;

    // Events and delegates
    public event ForwardHandler Forward;
    public delegate void ForwardHandler();
    public event StanceHandler StanceFinished;
    public delegate void StanceHandler();

    // Stance variables
    private int encampmentTracker;
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
        encampmentChance = 10;
        normalChance = 90;
        coinChance = 80;
        typeToBe = 0;
        // Chance this variable for difficulty
        maxEncampmentCount = 3;
        // Lerp variable initialization
        lerpTime = 0;
        rateOfLerp = 1.5f;
        disBeforeComplete = 0.25f;
        isLerping = false;
        newPosition = new Vector3(0,0,0);
        orgPosition = listOfPlatforms[0].transform.position;
        leftPosition = new Vector3(listOfPlatforms[0].transform.position.x + 4, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        centerPosition = new Vector3(listOfPlatforms[0].transform.position.x, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        rightPosition = new Vector3(listOfPlatforms[0].transform.position.x - 4, listOfPlatforms[0].transform.position.y, listOfPlatforms[0].transform.position.z);
        newPlatformPositions = new Vector3[6];
        // Stance variables
        encampmentTracker = 0;
        // GENERATE PLATFORMS
        // Call function to start making the path
        GeneratePlatforms();

        // Subscribe to event
        Subscribe(ms);
    }
    void Update()
    {
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
            listOfPlatforms[0].gameObject.SetActive(true);
            listOfPlatforms[0].transform.GetChild(2).gameObject.SetActive(false);
            GeneratePlatforms(1);
            isFirstPlatformActive = true;
        }
    }
    // Generate the platform locations
    void GeneratePlatforms(int newPlatformNumber)
    {
        for (int  i = 1; i < listOfPlatforms.Length; i++)
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
            typeToBe = GenerateTileType(platformType);
            // If the generated type is an encampment then trigger the function in the stance script attached to the child.
            if (typeToBe == 1)
            {
                // Keep track of the number of encampments spawned in a row.
                encampmentTracker++;
                listOfPlatforms[i].gameObject.transform.GetChild(typeToBe).GetComponent<StanceScript>();
            }
            else
            {
                encampmentTracker = 0;
            }
            listOfPlatforms[i].gameObject.transform.GetChild(typeToBe).gameObject.SetActive(true);
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
        go.transform.GetChild(GenerateTileType(platformType)).gameObject.SetActive(true);
        GenerateCoin(go, platformType);
        // Set the new starting point for next row generation.
        newPlatformStartPoint = nextRowPlatform;
    }
    // A function to decide if the tile generated is encampement or default.
    int GenerateTileType(int currentType)
    {
        // CURRENT TYPE 0 == DEFAULT, 1 == ENCAMPMENT
        // Decide which chance is going to be tested.
        int randomChance = Mathf.RoundToInt(UnityEngine.Random.Range(0, 100));
        // Below typeChance spawns type 0 above typeChance spawns type 1
        if (randomChance < encampmentChance && encampmentTileCount < 3)
        {
            platformType = 1;
            encampmentChance -= 5;
            encampmentTileCount++;
        }
        else
        {
            platformType = 0;
            encampmentChance += 5;
            encampmentTileCount = 0;
        }
        return platformType;
    }
    // Decide whether to spawn a coin or not.
    void GenerateCoin(GameObject go, int platformType)
    {
        if (platformType == 0)
        {
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0,100)) < coinChance)
            {
                go.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    // Function that reset the positions of object and score and basically starts the level again
    // TODO:NOTE:NOTE: Possibly update this, as when player dies there will be a shop screen, add something that pauses the game
    // UPDATE
    void ResetPlatforms() 
    {
        // Set the platforms back to the stored original positions.
        for (int i = 0; i < listOfPlatforms.Length; i++)
        {
            // Loop through the children and disable them.
            for (int j = 0; j < 2; j++)
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
        for (int i = 0; i < 2; i++)
        {
            go.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // Delegate to be called when the player presses forward
    void MovePlatformsForward()
    {
        if (!isLerping) {
            // Populate array with new Z positions for platforms.
            for (int i = 0; i < listOfPlatforms.Length; i++)
            {
                newPlatformPositions[i] = listOfPlatforms[i].transform.position;
                newPlatformPositions[i] = new Vector3(newPlatformPositions[i].x, newPlatformPositions[i].y, newPlatformPositions[i].z - 4);
            }
            isLerping = true;
            // Start the coroutine to lerp the platforms forward.
            StartCoroutine(LerpBackwards(newPlatformPositions));
        }
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // A function to move the plaforms based on input from MovementScript
    // COROUTINES
    public IEnumerator LerpBackwards(Vector3[] goPositions)
    {
        // Directions: 0 == FORWARD, 1 == RIGHT FORWARD, 2 == LEFT FORWARD
        while (isLerping)
        {
            lerpTime += Time.deltaTime * rateOfLerp;
            for (int i = 0; i < listOfPlatforms.Length; i++)
            {
                listOfPlatforms[i].transform.position = Vector3.Lerp(listOfPlatforms[i].transform.position, goPositions[i], lerpTime);
            }
            if (listOfPlatforms[5].transform.position.z <= goPositions[5].z + disBeforeComplete)
            {
                if (Forward != null)
                {
                    Forward();
                }
                // If the player is close enough to the final destination set the position.
                // NOTE: Fire a raycast to check if the player position is over a platform or not.
                for (int i = 0; i < listOfPlatforms.Length; i++)
               {
                    listOfPlatforms[i].transform.position = goPositions[i];
                }
                ms.CheckPlatformLanded();
                lerpTime = 0;
                isLerping = false;
                // Called after the position is set to speed up movement.
                MoveLastRow();
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(0);
        }
    }
    // Move the 3 platforms behind the player to the front
    public void MoveLastRow()
    {
        GameObject lastPlatform = null;
        Vector3 playerPosition = new Vector3(0,1,0);
        int position = 0;
        for (int i = 0; i < listOfPlatforms.Length; i++)
        {
            // UPDATE MOVEMENT SO THAT THE PLATFORMS ALL MOVE INSTEAD OF THE PARENT OBJECT
            if (listOfPlatforms[i].gameObject.transform.localPosition.z < playerPosition.z)
            {
                listOfPlatforms[i].gameObject.SetActive(false);
                listOfPlatforms[i].gameObject.transform.position = new Vector3(listOfPlatforms[i].gameObject.transform.position.x,
                                                                             listOfPlatforms[i].gameObject.transform.position.y,
                                                                             listOfPlatforms[i].gameObject.transform.localPosition.z + 24);
                lastPlatform = listOfPlatforms[i].gameObject;
                ResetChildren(lastPlatform);
                position = i;
                break;
            }
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
        ms.Move += new MovementScript.MovePlatform(MovePlatformsForward);
    }
}