using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
    // Health gameobjects
    GameObject healthTextMesh;
    // Health variables
    private int health;
    // Script Variables
    private MovementScript ms;
    // Events and delegates
    public event HealthResetHandler HealthReset;
    public delegate void HealthResetHandler(); 
	// Use this for initialization
	void Start () {
        // health variable initialization
        health = 100;
        // script variable intitalization
        ms = this.gameObject.GetComponent<MovementScript>();
        healthTextMesh = GameObject.FindGameObjectWithTag("health");
        // Subcribe to events
        Subscribe();
	}
    // A function to decrease the health of the player
    void ChangeHealth(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (HealthReset != null)
            {
                HealthReset();
            }
        }
        healthTextMesh.GetComponent<TextMesh>().text = health.ToString();
    }
    // A function to be called when an event reseting the level is called.
    void ResetHealth()
    {
        // Temp maxValue
        health = 100;
        healthTextMesh.GetComponent<TextMesh>().text = health.ToString();
    }
    // SUBSCRIPTIONS
    // Subscribe to events function
    void Subscribe()
    {
        ms.Health += new MovementScript.HealthHandler(ChangeHealth);
        ms.Reset += new MovementScript.ResetHandler(ResetHealth);
    }
}
