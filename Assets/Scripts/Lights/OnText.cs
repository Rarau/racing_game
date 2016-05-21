using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnText : MonoBehaviour {

    Countdown countdown;

    Text countdownText;

    int fontSize = 30;

    // Use this for initialization
    void Start () {
        countdown = transform.parent.GetComponent<Countdown>();
        countdownText = GetComponent<Text>();
        countdownText.text = "3";
        countdownText.fontSize = fontSize;
    }
	
	// Update is called once per frame
	void Update () {
        if ((int)countdown.timeLeft == 20) 
        {
            countdownText.text = "2";
            countdownText.fontSize = fontSize;
        }
        if ((int)countdown.timeLeft == 10)
        {
            countdownText.text = "1";
            countdownText.fontSize = fontSize;
        }
         if ((int)countdown.timeLeft <= 0)
        {
            countdownText.text = "GO!";
            if (Mathf.Abs((int)countdown.timeLeft) % 2 == 0)
                countdownText.fontSize = fontSize*2;
            else
                countdownText.fontSize = fontSize;
        }

        countdownText.fontSize += (int)((((100 + countdown.timeLeft)) / 100) );
    }
}
