using UnityEngine;
using System.Collections;

public class StanceTextScript : MonoBehaviour {
    private GameObject stanceText;
    private MovementScript ms;
    // Use this for initialization
    void Start(){
        ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        Subscribe();
    }
    // Function to write the text mesh to a string
    void DisplayStances(string stances)
    {
        this.GetComponent<TextMesh>().text = stances;
    }
    // Subscribe
    void Subscribe()
    {
        ms.ShowStance += new MovementScript.ShowStances(DisplayStances);
    }
}
