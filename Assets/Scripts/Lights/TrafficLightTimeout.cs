using UnityEngine;
using System.Collections;

public class TrafficLightTimeout : MonoBehaviour {

    Countdown countdown;

    // Use this for initialization
    void Start () {
        countdown = GameObject.Find("Countdown").GetComponent<Countdown>();

        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	    if (countdown.timeLeft < 0)
        {
            gameObject.SetActive(false);
        }
	}
}
