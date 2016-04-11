using UnityEngine;
using System.Collections;

public class StanceScript : MonoBehaviour {
    // Stance variables
    char[] stanceButtons = new char[2] { 'a', 'd' };
    char[] stances = new char[0];
    string stancesAsString;
    // Use this for initialization
    void Start () {
        stancesAsString = "";
    }
	// Update is called once per frame
	void Update () {
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // A function to generate the stances of an encampment
    public void GenerateStances(int encampmentNumber)
    {
        stancesAsString = "";
        // Depending on the encampment number generate different lengths of stances.
        if (encampmentNumber == 1)
        {
            // Randomly choose between either Q or E three times and store the chars in an array.
            stances = new char[3];
            for(int i = 0; i < 3; i++)
            {
                stances[i] = stanceButtons[Random.Range(0,2)];
            }
        }
        else if(encampmentNumber == 2)
        {
            // Randomly choose between either Q or E two times and store the chars in an array.
            stances = new char[2];
            for (int i = 0; i < stances.Length; i++)
            {
                stances[i] = stanceButtons[Random.Range(0, 2)];
            }
        }
        else if (encampmentNumber >= 3)
        {
            // Randomly choose between either Q or E once and store the char in an array.
            stances = new char[1];
            stances[0] = stanceButtons[Random.Range(0, 2)];
        }
        for (int i = 0; i < stances.Length; i++) {
            stancesAsString += stances[i];
        }
    }
    // A function to grab the stances of the platform associated with this script
    public char[] GrabStances()
    {
        return stances;
    }
    public string GrabStancesAsString()
    {
        return stancesAsString;
    }
}
