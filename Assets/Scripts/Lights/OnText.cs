using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnText : MonoBehaviour {

    Countdown countdown;

    Text countdownText;

    // Use this for initialization
    void Start () {
        countdown = transform.parent.GetComponent<Countdown>();
        countdownText = GetComponent<Text>();
        countdownText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
	    if (countdown.timeLeft >= 20)
        {
            countdownText.text = "3";
        }
        else if (countdown.timeLeft >= 10)
        {
            countdownText.text = "2";
        }
        else if (countdown.timeLeft >= 0)
        {
            countdownText.text = "1";
        }
        else
            countdownText.text = "GO!";
    }
}
