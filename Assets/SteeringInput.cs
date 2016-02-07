using UnityEngine;
using System.Collections;

public class SteeringInput : MonoBehaviour
{


    public WheelTest wheel1;
    public WheelTest wheel2;

    public float sensitivity = 0.01f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        wheel1.steeringAngle = sensitivity * Input.GetAxis("Horizontal");
        wheel2.steeringAngle = sensitivity * Input.GetAxis("Horizontal");
    }
}
