using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementScript : MonoBehaviour {
    // EVENTS
    // Reset level events
    public event ResetHandler Reset;
    public delegate void ResetHandler();
    // Increase score events
    public event ScoreHandler Score;
    public delegate void ScoreHandler();
    // Increase score by amount events
    public event ScoreIncreaseHandler ScoreIncrease;
    public delegate void ScoreIncreaseHandler(int amount);
    // Increase/Decrease health by amount events
    public event HealthHandler Health;
    public delegate void HealthHandler(int amount);
    // Move platforms event
    public event MovePlatform Move;
    public delegate void MovePlatform();
    // Stance text event
    public event ShowStances ShowStance;
    public delegate void ShowStances(string stance);
    // Lerping variables
    private float lerpTime;
    private float rateOfLerp;
    private float disBeforeComplete;
    private bool isLerping;
    private bool isLerpingForward;
    // Other script variables
    private PlatformsMovement pm;
    private HealthScript hs;
    private AttackScript attScript;
    // Stance variables
    private bool isAllowedToMove;
    private string stances;
    // Platform prediction
    private GameObject[] platforms;
    private int distanceToNextPlatform;
    private GameObject closestPlatform;
    private int nextPlatformDirection;
    private GameObject playerPlatform;
    private int removeIndex;
    private List<GameObject> removePlayerPlatformList;
    // An UI image of the stance background 
    public GameObject stanceBackground;
    void Start() {
        // lerping variable initialization
        lerpTime = 0;
        rateOfLerp = 1.5f;
        disBeforeComplete = 0.25f;
        isLerping = false;
        isLerpingForward = false;
        // grabbing script 
        pm = GameObject.FindGameObjectWithTag("platforms").GetComponent<PlatformsMovement>();
        hs = GameObject.FindGameObjectWithTag("health").GetComponent<HealthScript>();
        attScript = this.GetComponent<AttackScript>();
        // stance variables initialization
        isAllowedToMove = true;
        stances = "";
        platforms = new GameObject[6];
        playerPlatform = new GameObject();
        removeIndex = -1;
        removePlayerPlatformList = new List<GameObject>();
        // Value set to 100 so that accurate closest distance can be found
        distanceToNextPlatform = 100;
        nextPlatformDirection = -1;
        // Possible update this to have a default normal non-special type attack
        // Subscribe to event calls
        Subscribe();
    }
    void Update()
    {
        if (isAllowedToMove) {
            Movement();
        }
    }
    // A function called every frame to check if the user has decided to move.
    void Movement()
    {
        if (!isLerping && Input.GetKeyDown(KeyCode.A) && transform.position.x > -4 && !isLerpingForward)
        {
            isLerping = true;
            StartCoroutine(LerpSideways(new Vector3(transform.position.x - 4, transform.position.y, transform.position.z), 0));
            if (Move != null)
            {
                Move();
            }
        }
        else if (!isLerping && Input.GetKeyDown(KeyCode.D) && transform.position.x < 4 && !isLerpingForward)
        {
            isLerping = true;
            StartCoroutine(LerpSideways(new Vector3(transform.position.x + 4, transform.position.y, transform.position.z), 1));
            if (Move != null)
            {
                Move();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && !isLerpingForward)
        {
            isLerpingForward = true;
            if (Move != null)
            {
                Move();
            }
        }
    }
    // A function to change stance of the player
    // Collision detection functions
    // TODO: UPDATE
    void OnTriggerEnter(Collider col)
    {
        // If you've hit a coin
        if (col.gameObject.tag == "gold")
        {
            // Disable the coin from the screen for use later
            col.gameObject.SetActive(false);
            // Add the worth of the coin to the score
            if (Score != null)
            {
                Score();
            }
        }
    }
    // Set moving forward bool to false
    void SetForwardBool()
    {
        isLerpingForward = false;
    }
    // A function to call the reset event
    void ResetFunction()
    {
        // Allows the player to move again after dying from health loss.
        ResetMovement();
        if (Reset != null)
        {
            Reset();
        }
    }
    // A function to move the player automatically
    void MovePlayerAfterAttack()
    {
        if (closestPlatform.transform.position.x > this.gameObject.transform.position.x)
        {
            // Direction = right.
            nextPlatformDirection = 1;
        }
        else if (closestPlatform.transform.position.x < this.gameObject.transform.position.x)
        {
            // Direction = left.
            nextPlatformDirection = 0;
        }
        else
        {
            // Direction undecided - basically forward.
            nextPlatformDirection = -1;
        }
        // If a direction has been chosen lerp left or right towards it.
        if (nextPlatformDirection >= 0)
        {
            isLerping = true;
            StartCoroutine(LerpSideways(new Vector3(closestPlatform.transform.position.x, 1, 0), nextPlatformDirection));
        }
        // This is outside of the previous if statement so that if no direction is chosen it still moves forward.
        if (Move != null)
        {
            Move();
        }
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // COROUTINES
    public IEnumerator LerpSideways(Vector3 direction, int num)
    {
        // DIRECTIONS: 0 = left, 1 = right
        while (isLerping)
        {
            lerpTime += Time.deltaTime * rateOfLerp;
            transform.position = Vector3.Lerp(transform.position, direction, lerpTime);
            if ((transform.position.x < direction.x + disBeforeComplete && num == 0) ||
                (transform.position.x > direction.x - disBeforeComplete && num == 1))
            {
                transform.position = direction;
                isLerping = false;
                lerpTime = 0;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(0);
        }
    }
    // A coroutine for moving the player to the next closest platform
    public IEnumerator LerpPlayer(Vector3 newPosition)
    {
        while (isLerping)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, lerpTime);
        }
        yield return new WaitForSeconds(0);
    }
    // Fire raycast to check if player has jumped onto a platform
    public void CheckPlatformLanded()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10))
        {
            // If the player lands on an encampment, stop the player being able to move through isAllowedToMove 
            // Then start the event that tracks input.
            playerPlatform = hit.transform.gameObject;
            if (hit.transform.tag == "encampment")
            {
                isAllowedToMove = false;
                // Display the stances background NOTE: Must be improved to take into account the length of the stance attack.
                stanceBackground.SetActive(true);
                stances = hit.transform.gameObject.GetComponent<StanceScript>().GrabStancesAsString();
                if (ShowStance != null)
                {
                    ShowStance(stances);
                }
                // Here detect where the next platform is going to be,
                // And then fufcking do something about it johnny.
                platforms = pm.GrabPlatforms();
                // Making a list of the platforms to remove the platform the player is standing on.
                removePlayerPlatformList = new List<GameObject>(platforms);
                for (int i = 0; i < platforms.Length; i++) {
                    if (platforms[i].name == playerPlatform.name)
                    {
                        removeIndex = i;
                    }
                }
                removePlayerPlatformList.RemoveAt(removeIndex);
                platforms = removePlayerPlatformList.ToArray();
                // Remove the platform the player is on.
                // Reset search variables.
                closestPlatform = null;
                distanceToNextPlatform = 100;
                foreach (GameObject go in platforms)
                {
                    if (Vector3.Distance(this.gameObject.transform.position, go.transform.position) < distanceToNextPlatform)
                    {
                        distanceToNextPlatform = (int)Vector3.Distance(gameObject.transform.position, go.transform.position);
                        closestPlatform = go;
                    }
                }
                attScript.Attacking(hit.transform.gameObject.GetComponent<StanceScript>());
            }
        }
        // If the player misses a platform reset everything.
        if (!(Physics.Raycast(transform.position, -Vector3.up, out hit, 10)))
        {
            // If the event has a listener fire the event.
            if (Reset != null)
            {
                Reset();
            }
            transform.position = new Vector3(0, 1, 0);
        }
    }
    // A function for public use to allow movement to continue
    public void AllowMovement()
    {
        // A function call that makes the player move to the next platform.
        MovePlayerAfterAttack();
        isAllowedToMove = true;
    }
    public void ResetMovement()
    {
        isAllowedToMove = true;
    }
    // A method for resetting the active state of the stance background image
    public void ResetStanceBackgroundImage()
    {
        stanceBackground.SetActive(false);
    }
    // Subscribe to event function
    public void Subscribe()
    {
        pm.Forward += new PlatformsMovement.ForwardHandler(SetForwardBool);
        hs.HealthReset += new HealthScript.HealthResetHandler(ResetFunction);
    }
}