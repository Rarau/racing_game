using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarController))]
public class CarPlayerInput : MonoBehaviour
{
    private CarController carController;
    public string playerPrefix = "P1_";

    public AnimationCurve joystickSensitivity;
    public LookAtVelocity cameraRig;

    public int playerNum;
    public int totalPlayers;

	void Start () 
    {
        carController = GetComponent<CarController>();
	}
	
    
    

	void Update ()
    {
        cameraRig.rotOffset = Vector3.right * Input.GetAxis(playerPrefix + "Horizontal_2") + Vector3.up * Input.GetAxis(playerPrefix + "Vertical_2");

        carController.steeringAngle = joystickSensitivity.Evaluate(Mathf.Abs(Input.GetAxis(playerPrefix + "Horizontal"))) * Mathf.Sign((Input.GetAxis(playerPrefix + "Horizontal")));
        //if(Input.GetButton("HandBrake"))
        {
            carController.handBrake = Input.GetButton("HandBrake");
        }
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
