using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
    private float moveSpeed, lerpTime, rateOfLerp, disBeforeComplete;
    private bool isLerping;
    // Use this for initialization
    PlatformsMovement platMove = new PlatformsMovement();
    // Lerping and coroutine variables
    Vector3 newPosition;

    void Start() {
        // Grabbing the PlatformsMovement script from the Platforms game object
        moveSpeed = 4.0f;
        platMove = GameObject.FindGameObjectWithTag("platforms").GetComponent<PlatformsMovement>();
        // Initializing newPosition for use with movement
        newPosition = new Vector3();
        // Lerping variables
        lerpTime = 0.0f;
        rateOfLerp = 1f;
        disBeforeComplete = 0.1f;
        isLerping = false;
    }

    // Update is called once per frame
    void Update() {
        // Putting the call in update makes player movement faster.
        Movement();
    }
    void FixedUpdate()
    {
    }
    void Movement()
    {
        // Jump right and forward
        if (Input.GetKeyDown(KeyCode.D) && !isLerping && transform.position.x <= 0)
        {
            isLerping = true;
            platMove.StartCoroutine(platMove.MoveLastRow(transform.position));
            newPosition = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z + 4);
            StartCoroutine(LerpTowards(newPosition, 3));
        }
        // Jump left and forward
        else if (Input.GetKeyDown(KeyCode.A) && !isLerping && transform.position.x >= 0)
        {
            isLerping = true;
            platMove.StartCoroutine(platMove.MoveLastRow(transform.position));
            newPosition = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z + 4);
            StartCoroutine(LerpTowards(newPosition, 4));
        }
        /*// Jump right
        else if (Input.GetKeyDown(KeyCode.D) && !isLerping && transform.position.x <= 0)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
            StartCoroutine(LerpTowards(newPosition, 0));
        }
        // Jump left
        else if (Input.GetKeyDown(KeyCode.A) && !isLerping && transform.position.x >= 0)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
            StartCoroutine(LerpTowards(newPosition, 1));
        }*/
        // Jump forward
        else if (Input.GetKeyDown(KeyCode.W) && !isLerping)
        {
            isLerping = true;
            // NOTE: Possibly move the movelastrow into the lerptowards coroutine.
            platMove.StartCoroutine(platMove.MoveLastRow(transform.position));
            newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 4);
            StartCoroutine(LerpTowards(newPosition, 2));
        }
    }
    // COROUTINES
    public IEnumerator LerpTowards(Vector3 towardsPosition, int direction)
    {
        // Directions: 0 == LEFT, 1 == RIGHT, 2 == FORWARD, 3 == RIGHT FORWARD, 4 == LEFT FORWARD
        while (isLerping)
        {
            lerpTime += Time.deltaTime * rateOfLerp;
            transform.position = Vector3.Lerp(transform.position, towardsPosition, lerpTime);
            if ((transform.position.x >= towardsPosition.x - disBeforeComplete && direction == 0) ||
                (transform.position.x <= towardsPosition.x + disBeforeComplete && direction == 1) ||
                (transform.position.z >= towardsPosition.z - disBeforeComplete && direction == 2) ||
                (transform.position.x <= towardsPosition.x + disBeforeComplete && transform.position.z >= towardsPosition.z - disBeforeComplete && direction == 3) ||
                (transform.position.x >= towardsPosition.x - disBeforeComplete && transform.position.z >= towardsPosition.z - disBeforeComplete && direction == 4))
            {
                // If the player is close enough to the final destination set the position.
                // NOTE: Fire a raycast to check if the player position is over a platform or not.
                transform.position = towardsPosition;
                isLerping = false;
                lerpTime = 0;
            }
            yield return new WaitForSeconds(0);
        }
    }
}
