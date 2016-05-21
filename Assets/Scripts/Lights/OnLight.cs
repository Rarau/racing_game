using UnityEngine;
using System.Collections;

public class OnLight : MonoBehaviour {

    Countdown countdown;

    public bool red;
    public bool yellow;
    public bool green;
    Countdown.signal signal;
    Renderer renderer;
    Material material;

    Color myColor;

    // Use this for initialization
    void Start () {
        //countdown = transform.parent.parent.GetComponent<Countdown>();

        countdown = GameObject.Find("Countdown").GetComponent<Countdown>();

        renderer = GetComponent<Renderer>();
        material = renderer.material;

        if (red) {
            material.SetColor("_EmissionColor", Color.grey);
            myColor = Color.red;
            signal = Countdown.signal.RED;
        } 
        if (yellow) {
            material.SetColor("_EmissionColor", Color.grey);
            myColor = Color.yellow;
            signal = Countdown.signal.YELLOW;
        } 
        if (green) {
            material.SetColor("_EmissionColor", Color.grey);
            myColor = Color.green;
            signal = Countdown.signal.GREEN;
        }
        material.SetFloat("_EmissionScaleUI", 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
	    if (countdown.mySignal == signal)
        {
            material.SetColor("_EmissionColor", myColor);
        }
        else
        {
            material.SetColor("_EmissionColor", Color.grey);
        }
	}
}
