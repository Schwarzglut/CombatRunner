using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
    // Health variables
    private int health;
    // Script Variables
    private MovementScript ms;
	// Use this for initialization
	void Start () {
        // health variable initialization
        health = 100;
        // script variable intitalization
        ms = this.gameObject.GetComponent<MovementScript>();
        // Subcribe to events
        Subscribe();
	}
    // A function to decrease the health of the player
    void ChangeHealth(int amount)
    {
        health -= amount;
        Debug.Log(health);
    }
    // A function to be called when an event reseting the level is called.
    void ResetHealth()
    {
        // Temp maxValue
        health = 100;
    }
    // SUBSCRIPTIONS
    // Subscribe to events function
    void Subscribe()
    {
        ms.Health += new MovementScript.HealthHandler(ChangeHealth);
        ms.Reset += new MovementScript.ResetHandler(ResetHealth);
    }
}
