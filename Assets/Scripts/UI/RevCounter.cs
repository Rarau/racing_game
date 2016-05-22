using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RevCounter : MonoBehaviour 
{
    public CarController car;
    Image pointer;
    public float minAnglePointer = 0.0f;
    public float maxAnglePointer = 240.0f;
    Text gear;

	// Use this for initialization
	void Start () 
    {
        pointer = transform.FindChild("Pointer").GetComponent<Image>();
        gear = transform.FindChild("GearIndicator").GetComponent<Text>();
        //pointer.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, minAnglePointer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (car == null)
            return;
        float speedFactor = car.currentSpeed / car.maxSpeed;
        float rpmFactor = car.virtualRPM;
        float rotationAngle;
        gear.text = car.currentGear.ToString();
        if (car.currentSpeed >= 0)
        {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, rpmFactor);
        }
        else
        {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, -rpmFactor);
        }
        pointer.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -rotationAngle);
        //GUIUtility.RotateAroundPivot(rotationAngle, pivotPoint);
        //Debug.Log("rpmFactor: " + car.rpmMax);
        //Debug.Log("virtualRpm: " + car.virtualRPM);
        //Debug.Log("rotationAngle: " + rotationAngle);
	}
}
