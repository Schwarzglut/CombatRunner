using UnityEngine;
using System.Collections;

public class StreakBonusFall : MonoBehaviour {
    // Script variables
    private ScoreScript ss;
    // Lerping variables
    private float lerpTime, rateOfLerp;
    private Vector3 streakStartPosition, streakFallPosition, startPosition;
    // Use this for initialization
    void Start () {
        // Script initialization
        ss = GameObject.FindGameObjectWithTag("score").GetComponent<ScoreScript>();
        // Lerping initialization
        lerpTime = 0;
        rateOfLerp = 1.5f;
        startPosition = transform.localPosition;
        streakFallPosition = startPosition;
        // Subscribing
        Subscribe(ss);
	}
	// Update is called once per frame
	void Update () {
	}
    void ResetStreakFall()
    {
        // Set the gameobject invisible.
        Color alpha = new Color(0,0,0,0);
        this.gameObject.GetComponent<TextMesh>().color = alpha;
        // Return the score to its original position for use again.
        this.gameObject.transform.localPosition = streakFallPosition = startPosition;
    }
    void SetDropStreakAmount()
    {
        streakStartPosition = new Vector3(-1,1,10);
        // Set the new random starting position.
        streakStartPosition = new Vector3(streakStartPosition.x + Random.Range(-7.50f, 6.50f), streakStartPosition.y + Random.Range(-4f, 5f), streakStartPosition.z);
        // Set the new fall amount.
        streakFallPosition = new Vector3(streakStartPosition.x, streakStartPosition.y - 2, streakStartPosition.z);
        // Start coroutines.
        StartCoroutine(DropStreakAmount(streakStartPosition, streakFallPosition));
        StartCoroutine(DropStreakAmountAlpha());
    }
    // !!------ PUBLIC FUNCTIONS ------!!
    // COROUTINES
    public IEnumerator DropStreakAmount(Vector3 streakStart, Vector3 streakFall)
    {
        transform.localPosition = streakStart;
        while (lerpTime < 0.5f)
        {
            lerpTime += Time.deltaTime * rateOfLerp;
            transform.localPosition = Vector3.Lerp(streakStart, streakFall, lerpTime);
            yield return new WaitForSeconds(0);
        }
        ResetStreakFall();
        lerpTime = 0;
    }
    public IEnumerator DropStreakAmountAlpha()
    {
        Color alpha = new Color(0, 0, 0, 255);
        this.GetComponent<TextMesh>().color = alpha;
        while (alpha.a > 0.5f)
        {
            alpha.a = Mathf.Lerp(this.GetComponent<TextMesh>().color.a, 0, lerpTime);
            this.gameObject.GetComponent<TextMesh>().color = alpha;
            yield return new WaitForSeconds(0);
        }
    }
    // SUBSCRIPTIONS
    // Subscribe to score event
    public void Subscribe(ScoreScript ss)
    {
        ss.Streak += new ScoreScript.StreakHandler(SetDropStreakAmount);
    }
    // Unsubscribe (unsubscribe if listener outlives event caller to break link for garbage collection)
    public void Unsubscribe(ScoreScript ss)
    {
        ss.Streak -= new ScoreScript.StreakHandler(SetDropStreakAmount);
    }
}
