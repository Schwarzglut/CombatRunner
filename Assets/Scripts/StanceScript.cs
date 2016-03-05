using UnityEngine;
using System.Collections;

public class StanceScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Input.inputstring to detect what the player has pressed and compare the char's with each char in the generated string of stances.
        if (Input.inputString != "")
        {
            char temp = Input.inputString[0];
            print(temp);
        }
    }
    // A function to generate the stances of an encampment
    void GenerateStances(int encampmentNumber)
    {
        // Depending on the encampment number generate different lengths of stances.
        if (encampmentNumber == 1)
        {
            // Randomly choose between either Q or E three times and store the chars in an array.
        }
        else if(encampmentNumber == 2)
        {
            // Randomly choose between either Q or E two times and store the chars in an array.
        }
        else if (encampmentNumber >= 3)
        {
            // Randomly choose between either Q or E once and store the char in an array.
        }
    }
}
