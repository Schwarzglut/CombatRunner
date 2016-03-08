using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {
    // Stance variables
    char[] stances = new char[0];
    // Health reducing event
    public event HealthHandler ReduceHealth;
    public delegate void HealthHandler(int amount);
    // Other variables
    MovementScript ms;
    // Use this for initialization
	void Start () {
        ms = this.GetComponent<MovementScript>();
	}
	// Update is called once per frame
	void Update () {
	}
    // !!------ PUBLIC FUNCTIONS ------!!
    // A function to track the input of the user when on a platform
    public void Attacking(StanceScript ss)
    {
        stances = new char[ss.GrabStances().Length];
        stances = ss.GrabStances();
        StartCoroutine(AttackingCoroutine());
    }
    public IEnumerator AttackingCoroutine()
    {
        bool finished = false;
        int position = 0;
        while (!finished)
        {
            if (Input.inputString != "")
            {
                if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
                {
                    if (Input.inputString[0] == stances[position])
                    {
                        position++;
                        if (position == stances.Length)
                        {
                            finished = true;
                            ms.AllowMovement();
                        }
                    }
                    else
                    {
                        position = 0;
                        if (ReduceHealth != null)
                        {
                            ReduceHealth(20);
                        }
                        //TODO: Some event that reduces health? 
                    }
                }
            }
            yield return new WaitForSeconds(0);
        }
    }
}
