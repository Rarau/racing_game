using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RevCounter : MonoBehaviour 
{
    public CarController car;
    Image pointer;
    public float minAnglePointer = 80.0f;
    public float maxAnglePointer = 360.0f;

	// Use this for initialization
	void Start () 
    {
        pointer = transform.FindChild("Pointer").GetComponent<Image>();
        //pointer.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, minAnglePointer);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float speedFactor = car.currentSpeed / car.maxSpeed;
        float rpmFactor = car.virtualRPM / car.rpmMax;
        float rotationAngle;
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
	}
}
