using UnityEngine;
using System.Collections;

public class PlatformHealthScript : MonoBehaviour {
    private float platformHealth;
    // Platform type, 0 == Normal, 1 == Fire, 2 == Water, 3 == Earth, 4 == Wind
    private int platformType;
    // Int to trace the series of platforms
    private int platformTypeTracker;
	// Use this for initialization
	void Start () {
        // COMPLETELY TEMP HEALTH.
        platformHealth = 100;
        // Initializate the platform type to normal.
        platformType = 0;
        // Initializatie the platform type tracker.
        platformTypeTracker = 0;
	}
	// Update is called once per frame
	void Update () {
	
	}
    // A function to set the type of platform
    void SetPlatformType(int type)
    {
        platformType = type;
    }
    // Generate platform type
    void GeneratePlatformType()
    {

    }
}
