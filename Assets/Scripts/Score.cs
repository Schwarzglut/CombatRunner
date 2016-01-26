using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
    private GameObject scoreValue;
    private MovementScript ms;
    int score;
    // Use this for initialization
	void Start () {
        ms = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        scoreValue = GameObject.FindGameObjectWithTag("score");
        score = 0;
        Subscribe(ms);
	}
    void UpdateScore()
    {
        score++;
        scoreValue.GetComponent<TextMesh>().text = "Score: " + score.ToString();
    }
    void UpdateScore(int incrementAmount)
    {
        score = incrementAmount;
        scoreValue.GetComponent<TextMesh>().text = "Score: " + score.ToString();
    }
    void ResetScore()
    {
        score = 0;
        scoreValue.GetComponent<TextMesh>().text = "Score: " + score.ToString();
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    public void Subscribe(MovementScript ms)
    {
        ms.Score += new MovementScript.ScoreHandler(UpdateScore);
        ms.Reset += new MovementScript.ResetHandler(ResetScore);
    }
    public void Unsubscribe(MovementScript ms)
    {
        ms.Score -= new MovementScript.ScoreHandler(UpdateScore);
        ms.Reset -= new MovementScript.ResetHandler(ResetScore);
    }
}
