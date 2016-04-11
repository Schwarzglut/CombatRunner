using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {
    // Stance variables
    char[] stances = new char[0];
    // Health reducing event
    public event HealthHandler ReduceHealth;
    public delegate void HealthHandler(int amount);
    // Finish attack event
    public event AttackHandler FinishedAttack;
    public delegate void AttackHandler();
    // Other variables
    MovementScript movScript;
    // Use this for initialization
	void Start () {
        movScript = GetComponent<MovementScript>();
        Subscribe(movScript);
    }
	// Update is called once per frame
	void Update () {
	}
    // A function to stop coroutines if the game resets during attacking
    void ResetAttack()
    {
        StopAllCoroutines();
        movScript.ResetMovement();
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
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    if (Input.inputString[0] == stances[position])
                    {
                        position++;
                        if (position == stances.Length)
                        {
                            finished = true;
                            movScript.AllowMovement();
                            if (FinishedAttack != null) {
                                FinishedAttack();
                            }
                        }
                    }
                    else
                    {
                        position = 0;
                        if (ReduceHealth != null)
                        {
                            ReduceHealth(20);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0);
        }
    }
    public void Subscribe(MovementScript ms)
    {
        ms.Reset += new MovementScript.ResetHandler(ResetAttack);
    }
}
