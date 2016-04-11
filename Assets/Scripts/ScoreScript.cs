using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
    /*
    *
    *   Strip everything STREAK related
    *   Add new STREAK system for completing attacks
    *
    */
    // Score and streak variables
    private GameObject scoreValue, streakValue;
    private MovementScript movScript;
    private int score;
    // Lerp variables
    private float lerpTime, rateOfLerp;
    private Vector3 streakFallPosition, startPosition, streakStartPosition;
    // Events and delegates
    public event StreakHandler Streak;
    public delegate void StreakHandler();
    // Use this for initialization
    void Start () {
        movScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        streakValue = GameObject.FindGameObjectWithTag("streak");
        // Score variable initialization
        score = 0;
        // Lerp variable initialization
        lerpTime = 0;
        rateOfLerp = 1;
        // Streak fall initialization
        startPosition = transform.localPosition;
        streakFallPosition = startPosition;
        // Subscribing to events
        Subscribe(movScript);
	}
    // A function that simply increases the score by 1
    void UpdateScore()
    {
        // Increment score and streak tracker, if you get 5(temp) right in a row thats worth a streak bonus,
        // which is worth 10
        score++;
        GetComponent<UnityEngine.UI.Text>().text = score.ToString();
    }
    // Overriden function for an input parameter of the int
    void UpdateScore(int incrementAmount)
    {
        score += incrementAmount;
        if (Streak != null)
        {
            Streak();
        }
        GetComponent<UnityEngine.UI.Text>().text = score.ToString();
    }
    void ResetScore()
    {
        // TODO: Update highscore system
        // If the score achieved is greater than the current highscore overwrite it with the new score.
        if (PlayerPrefs.GetFloat("highscore") < score)
        {
            PlayerPrefs.SetFloat("highscore", score);
        }
        // Reset the score variables.
        score = 0;
        GetComponent<UnityEngine.UI.Text>().text = score.ToString();
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // SUBSCRIPTIONS
    // Subscribe to score and reset event
    public void Subscribe(MovementScript ms)
    {
        ms.Score += new MovementScript.ScoreHandler(UpdateScore);
        ms.ScoreIncrease += new MovementScript.ScoreIncreaseHandler(UpdateScore);
        ms.Reset += new MovementScript.ResetHandler(ResetScore);
    }
}
