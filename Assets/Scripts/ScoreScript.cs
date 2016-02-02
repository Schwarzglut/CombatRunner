using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
    // Score and streak variables
    private GameObject scoreValue, streakValue;
    private MovementScript ms;
    private int streakTracker, streakTrackerMax, streakAmount, score;
    // Lerp variables
    private float lerpTime, rateOfLerp;
    private Vector3 streakFallPosition, startPosition, streakStartPosition;
    // Events and delegates
    public event StreakHandler Streak;
    public delegate void StreakHandler();
    // Use this for initialization
    void Start () {
        ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        scoreValue = GameObject.FindGameObjectWithTag("score");
        streakValue = GameObject.FindGameObjectWithTag("streak");
        // Score variable initialization
        score = 0;
        streakTracker = 0;
        streakTrackerMax = 5;
        streakAmount = 10;
        // Lerp variable initialization
        lerpTime = 0;
        rateOfLerp = 1;
        // Streak fall initialization
        startPosition = transform.localPosition;
        streakFallPosition = startPosition;
        // Subscribing to events
        Subscribe(ms);
	}
    // A function that simply increases the score by 1
    void UpdateScore()
    {
        // Increment score and streak tracker, if you get 5(temp) right in a row thats worth a streak bonus,
        // which is worth 10
        score++;
        streakTracker++;
        if (streakTracker >= 5)
        {
            streakTracker = 0;
            // If the event has a listener fire the event.
            if (Streak != null)
            {
                Streak();
            }
            score += streakAmount;
        }
        scoreValue.GetComponent<TextMesh>().text = score.ToString();
    }
    // Overriden function for an input parameter of the int
    void UpdateScore(int incrementAmount)
    {
        score = incrementAmount;
        scoreValue.GetComponent<TextMesh>().text = score.ToString();
    }
    void ResetScore()
    {
        // If the score achieved is greater than the current highscore overwrite it with the new score.
        if (PlayerPrefs.GetFloat("highscore") < score)
        {
            PlayerPrefs.SetFloat("highscore", score);
        }
        // Reset the score variables.
        streakTracker = 0;
        score = 0;
        scoreValue.GetComponent<TextMesh>().text = score.ToString();
    }
    // SUBSCRIPTIONS
    // Subscribe to score and reset event
    public void Subscribe(MovementScript ms)
    {
        ms.Score += new MovementScript.ScoreHandler(UpdateScore);
        ms.Reset += new MovementScript.ResetHandler(ResetScore);
    }
    // Unsubscribe (unsubscribe if listener outlives event caller to break link for garbage collection)
    public void Unsubscribe(MovementScript ms)
    {
        ms.Score -= new MovementScript.ScoreHandler(UpdateScore);
        ms.Reset -= new MovementScript.ResetHandler(ResetScore);
    }
}
