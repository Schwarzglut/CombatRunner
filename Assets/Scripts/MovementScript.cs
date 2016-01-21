using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
    private float moveSpeed, lerpTime, rateOfLerp;
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
        rateOfLerp = 0.8f;
        isLerping = false;
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("Player position: " + transform.position);
    }
    void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        // Jump right and forward
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.W) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z + 4);
            StartCoroutine(LerpTowards(newPosition, 3));
        }
        // Jump left and forward
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.W) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z + 4);
            StartCoroutine(LerpTowards(newPosition, 4));
        }
        // Jump right
        else if (Input.GetKeyDown(KeyCode.D) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
            StartCoroutine(LerpTowards(newPosition, 0));
        }
        // Jump left
        else if (Input.GetKeyDown(KeyCode.A) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
            StartCoroutine(LerpTowards(newPosition, 1));
        }
        // Jump forward
        else if (Input.GetKeyDown(KeyCode.W) && !isLerping)
        {
            isLerping = true;
            newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 4);
            platMove.MoveLastRow(newPosition);
            StartCoroutine(LerpTowards(newPosition, 2));
        }

    }
    // COROUTINES
    public IEnumerator LerpTowards(Vector3 towardsPosition, int direction)
    {
        // Directions: 0 == LEFT, 1 == RIGHT, 2 == FORWARD, 3 == RIGHT FORWARD, 4 == LEFT FORWARD
        while (isLerping)
        {
            //Debug.Log("Lerping like a fucker");
            lerpTime += Time.deltaTime * rateOfLerp;
            transform.position = Vector3.Lerp(transform.position, towardsPosition, lerpTime);
            if ((transform.position.x >= towardsPosition.x - 0.05f && direction == 0) || 
                (transform.position.x <= towardsPosition.x + 0.05f && direction == 1) ||  
                (transform.position.z >= towardsPosition.z - 0.05f && direction == 2) || 
                (transform.position.x <= towardsPosition.x + 0.05f && transform.position.z >= towardsPosition.z - 0.05f && direction == 3) ||
                (transform.position.x >= towardsPosition.x - 0.05f && transform.position.z >= towardsPosition.z - 0.05f && direction == 4))
            {
                transform.position = towardsPosition;
                isLerping = false;
                lerpTime = 0;
                StopCoroutine(LerpTowards(newPosition, direction));
            }
            yield return new WaitForSeconds(0);
        }
    }
}
