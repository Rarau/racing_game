using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarController))]
public class CarPlayerInput : MonoBehaviour
{
    private CarController carController;
    public string playerPrefix = "P1_";

	void Start () 
    {
        carController = GetComponent<CarController>();
	}
	
	void Update ()
    {
        carController.steeringAngle = Input.GetAxis(playerPrefix + "Horizontal");
        carController.throttlePos = Input.GetAxis(playerPrefix + "Vertical");
	}
}
