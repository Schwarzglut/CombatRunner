using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StanceTextScript : MonoBehaviour {
    private GameObject player;

    private MovementScript movScript;
    private AttackScript attScript;
    private HealthScript healScript;
    // Stance image gameobjects
    public GameObject leftStance, midStance, rightStance;
    public GameObject[] arrayOfStanceGameObjects = new GameObject[3];
    // Array of sprites
    public Sprite[] stanceSprites = new Sprite[2];
    // Use this for initialization
    void Start(){
        arrayOfStanceGameObjects[0] = leftStance; arrayOfStanceGameObjects[1] = midStance; arrayOfStanceGameObjects[2] = rightStance;
        player = GameObject.FindGameObjectWithTag("Player");
        movScript = player.GetComponent<MovementScript>();
        attScript = player.GetComponent<AttackScript>();
        healScript = GameObject.FindGameObjectWithTag("health").GetComponent<HealthScript>();
        Subscribe();
    }
    // Function to write the text mesh to a string
    void DisplayStances(string stances)
    {
        // Loop through the stances and change the image in either left,mid or right based on the string stances.
        SetStanceImages(stances.Length);
        for (int i = 0; i < stances.Length; i++)
        {
            if (stances[i].Equals('a'))
            {
                arrayOfStanceGameObjects[i].GetComponent<Image>().sprite = stanceSprites[0];
            }
            else
            {
                arrayOfStanceGameObjects[i].GetComponent<Image>().sprite = stanceSprites[1];
            }
        }
    }
    // A function to clear the stance text
    void ClearStances()
    {
        ResetStancesImages();
        movScript.ResetStanceBackgroundImage();
    }
    // A function to set a select number of sprites to active
    void SetStanceImages(int num)
    {
        switch (num)
        {
            case 1:
                leftStance.SetActive(true);
                break;
            case 2:
                leftStance.SetActive(true);
                midStance.SetActive(true);
                break;
            case 3:
                leftStance.SetActive(true);
                midStance.SetActive(true);
                rightStance.SetActive(true);
                break;
            default:
                break;
        }
    }
    // A function to reset all stance images to false
    void ResetStancesImages()
    {
        leftStance.SetActive(false); midStance.SetActive(false); rightStance.SetActive(false);
    }
    // Subscribe
    void Subscribe()
    {
        movScript.ShowStance += new MovementScript.ShowStances(DisplayStances);
        attScript.FinishedAttack += new AttackScript.AttackHandler(ClearStances);
        healScript.HealthReset += new HealthScript.HealthResetHandler(ClearStances);
    }
}
