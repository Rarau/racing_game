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
        if (Input.GetAxis(playerPrefix + "Vertical") == 0.0f)
        {
            carController.throttlePos = 0.0f;
            carController.brakePos = 0.0f;
        }
        else
        {

                if (Input.GetAxis(playerPrefix + "Vertical") > 0.0f)
                {
                    carController.throttlePos = Input.GetAxis(playerPrefix + "Vertical");
                }
                else
                {
                    if (carController.ForwardVelocity > 0.0f)
                    {
                        carController.brakePos = Input.GetAxis(playerPrefix + "Vertical");
                    }
                    else
                    {
                        carController.throttlePos = Input.GetAxis(playerPrefix + "Vertical");
                    }
            }
            

        }

    }
}
