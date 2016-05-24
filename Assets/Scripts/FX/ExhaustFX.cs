using UnityEngine;
using System.Collections;

public class ExhaustFX : MonoBehaviour 
{
    CarController carController;
    int previousGear;
    public GameObject exhaust;


    void Start()
    {
        carController = transform.root.GetComponent<CarController>();
    }

	// Use this for initialization
	void OnEnable () 
    {
        previousGear = 1;
        if(carController == null)
            carController = transform.root.GetComponent<CarController>();
        carController.gearShiftEvent += OnGearShift;
	}

    void OnGearShift(int gearNumber)
    {
        if (previousGear > gearNumber)
        {
            if (carController.currentSpeed > 40.0f)
            {
                exhaust.SetActive(true);
            }
        }
        previousGear = gearNumber;
    }

    void OnDisable()
    {
        carController.gearShiftEvent -= OnGearShift;
    }
}
