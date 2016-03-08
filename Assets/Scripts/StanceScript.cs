using UnityEngine;
using System.Collections;

public class StanceScript : MonoBehaviour {
    // Stance variables
    char[] stanceButtons = new char[2] { 'q', 'e' };
    char[] stances = new char[0];
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Input.inputstring to detect what the player has pressed and compare the char's with each char in the generated string of stances.
        /*if (Input.inputString != "")
        {
            char temp = Input.inputString[0];
            print(temp);
        }*/
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // A function to generate the stances of an encampment
    public void GenerateStances(int encampmentNumber)
    {
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
        string temp = "";
        for (int i = 0; i < stances.Length; i++) {
            temp += stances[i];
        }
        Debug.Log(temp);
    }
    // A function to grab the stances of the platform associated with this script
    public char[] GrabStances()
    {
        return stances;
    }
}
