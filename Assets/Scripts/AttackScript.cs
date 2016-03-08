using UnityEngine;
using System.Collections;

public class AttackScript : MonoBehaviour {
    char[] stances = new char[0];
    // Use this for initialization
	void Start () {
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
            if (Input.inputString[0] == stances[position])
            {
                position++;
                if (position == stances.Length)
                {
                    finished = true;
                }
            }
            else
            {
                position = 0;
                //TODO: Some event that reduces health? 
            }
        }
        yield return new WaitForSeconds(0);
    }
}
