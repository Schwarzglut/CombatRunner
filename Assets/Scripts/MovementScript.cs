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
    void Start() {
    }
    // Fire raycast to check if player has jumped onto a platform
    public void CheckPlatformLanded()
    {
        RaycastHit hit = new RaycastHit();
        if (!(Physics.Raycast(transform.position, -Vector3.up, out hit, 10)))
        {
            // If the event has a listener fire the event.
            if (Reset != null) {
                Reset();
            }
        }
    }
    // Collision detection functions
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
}
