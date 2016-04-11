using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
    // Player gameobject
    private GameObject player;
    // Health variables
    private int health;
    // Script Variables
    private MovementScript movScript;
    private AttackScript attScript;
    // Events and delegates
    public event HealthResetHandler HealthReset;
    public delegate void HealthResetHandler(); 
	// Use this for initialization
	void Start () {
        // health variable initialization.
        health = 100;
        // Grab the player gameobject.
        player = GameObject.FindGameObjectWithTag("Player");
        // script variable intitalization.
        movScript = player.GetComponent<MovementScript>();
        attScript = player.GetComponent<AttackScript>();
        // Subcribe to events.
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
        GetComponent<UnityEngine.UI.Text>().text = health.ToString();
    }
    // A function to be called when an event reseting the level is called
    void ResetHealth()
    {
        // Temp maxValue
        health = 100;
        GetComponent<UnityEngine.UI.Text>().text = health.ToString();
    }
    // A function that resets the position of the player
    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0,1,0);
    }
    // SUBSCRIPTIONS
    // Subscribe to events function
    void Subscribe()
    {
        movScript.Health += new MovementScript.HealthHandler(ChangeHealth);
        attScript.ReduceHealth += new AttackScript.HealthHandler(ChangeHealth);
        movScript.Reset += new MovementScript.ResetHandler(ResetHealth);
        movScript.Reset += new MovementScript.ResetHandler(ResetPlayerPosition);
    }
}
