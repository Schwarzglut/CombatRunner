using UnityEngine;
using System.Collections;

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
    void Start() {
        // lerping variable initialization
        lerpTime = 0;
        rateOfLerp = 1.5f;
        disBeforeComplete = 0.25f;
        isLerping = false;
        isLerpingForward = false;
        // grabbing script 
        pm = GameObject.FindGameObjectWithTag("platforms").GetComponent<PlatformsMovement>();
        hs = this.GetComponent<HealthScript>();
        attScript = this.GetComponent<AttackScript>();
        // stance variables initialization
        isAllowedToMove = true;
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
    // A function to record the players stance changes while on a platform
    void StaceSteps()
    {
        // Find a way to record player input or at least match it to an aray.
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
    // A function to reduce health and reward gold
    // A function to call the reset event
    void ResetFunction()
    {
        if (Reset != null)
        {
            Reset();
        }
    }
    // A function to allow the player to start moving again
    void SetAllowPlayerMovement()
    {
        isAllowedToMove = true;
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
    // Fire raycast to check if player has jumped onto a platform
    public void CheckPlatformLanded()
    {
        // TODO: Add functionality to check if platform landed upon is an encampment
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10))
        {
            // If the player lands on an encampment, stop the player being able to move though isAllowedToMove 
            // Then start the event that tracks input and 
            if (hit.transform.tag == "encampment")
            {
                isAllowedToMove = false;
                attScript.Attacking(hit.transform.gameObject.GetComponent<StanceScript>());
            }
        }
        if (!(Physics.Raycast(transform.position, -Vector3.up, out hit, 10)))
        {
            // If the event has a listener fire the event.
            if (Reset != null)
            {
                Reset();
            }
            // Reset the position of the player.
            transform.position = new Vector3(0, 1, 0);
        }
    }
    // A function for public use to allow movement to continue
    public void AllowMovement()
    {
        isAllowedToMove = true;
    }
    // Subscribe to event function
    public void Subscribe()
    {
        pm.Forward += new PlatformsMovement.ForwardHandler(SetForwardBool);
        hs.HealthReset += new HealthScript.HealthResetHandler(ResetFunction);
    }
}
