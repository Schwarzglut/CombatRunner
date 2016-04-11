using UnityEngine;
using System.Collections;

public class StanceTextScript : MonoBehaviour {
    private GameObject player;

    private GameObject stanceText;
    private MovementScript movScript;
    private AttackScript attScript;
    private HealthScript healScript;
    // Use this for initialization
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        movScript = player.GetComponent<MovementScript>();
        attScript = player.GetComponent<AttackScript>();
        healScript = GameObject.FindGameObjectWithTag("health").GetComponent<HealthScript>();
        GetComponent<UnityEngine.UI.Text>().text = "";
        Subscribe();
    }
    // Function to write the text mesh to a string
    void DisplayStances(string stances)
    {
        GetComponent<UnityEngine.UI.Text>().text = "";
        GetComponent<UnityEngine.UI.Text>().text = stances;
    }
    // A function to clear the stance text
    void ClearStances()
    {
        GetComponent<UnityEngine.UI.Text>().text = "";
    }
    // Subscribe
    void Subscribe()
    {
        movScript.ShowStance += new MovementScript.ShowStances(DisplayStances);
        attScript.FinishedAttack += new AttackScript.AttackHandler(ClearStances);
        healScript.HealthReset += new HealthScript.HealthResetHandler(ClearStances);
    }
}
