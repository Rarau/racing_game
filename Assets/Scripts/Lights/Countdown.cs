using UnityEngine;
using System.Collections;
using System;

public class Countdown : MonoBehaviour {

    public float timeLeft = 30.0f;

    public enum signal { RED, YELLOW, GREEN};
    public signal mySignal;
    public event Action countdownFinishedEvent;

    bool expired = false;

    // Use this for initialization
    void Start () {

        expired = false;
        mySignal = signal.RED;
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

        timeLeft -= Time.deltaTime*4;

        if (expired)
            return;

        if (timeLeft >= 20)
        {           
            mySignal = signal.RED;
        }
        else if (timeLeft >= 10 && timeLeft < 20)
        {
            mySignal = signal.YELLOW;
        }
        else if (timeLeft >= 0 && timeLeft < 10)
        {
            mySignal = signal.GREEN;
        }

        if (timeLeft < 0)
        {
            expired = true;
            if (countdownFinishedEvent != null)
            {
                countdownFinishedEvent();
            }
        }
    }
}
