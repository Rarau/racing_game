using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class SpeedCounter : MonoBehaviour
{
    public CarController car;
    //public Rigidbody rb;

    private Text t;

    // Use this for initialization
    void Start()
    {
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t.text = (car.currentSpeed).ToString("0.0 KMH") + "\n";
        t.text += ((car.currentSpeed)*.62f).ToString("0.0 MPH") + "\n";
        t.text += "Engine rpm: " + car.myCurrentRPM + "\n";
        t.text += "Gear: " + car.currentGear + "\n";
    }
}
