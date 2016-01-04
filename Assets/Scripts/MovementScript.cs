using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
    private float moveSpeed;
    // Use this for initialization
    void Start () {
        moveSpeed = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
    }
    void Movement()
    {
        // Jump right
        if (Input.GetKeyUp(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed);
        }
        // Jump left
        else if (Input.GetKeyUp(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed);
        }
        // Jump forward
        else if (Input.GetKeyUp(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed);
        }
    }
}
