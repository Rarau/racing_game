using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MilesCounter : MonoBehaviour 
{
    public CarController car;
    Image pointer;
    public float minAnglePointer = 0.0f;
    public float maxAnglePointer = 280.0f;
    Text miles;

	// Use this for initialization
	void Start () 
    {
        pointer = transform.FindChild("Pointer").GetComponent<Image>();
        miles = transform.FindChild("MilesIndicator").GetComponent<Text>();
        //pointer.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, minAnglePointer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float speedFactor = car.currentSpeed / car.maxSpeed * .4f;
        //float rpmFactor = car.virtualRPM / car.rpmMax;
        float rotationAngle;
        miles.text = ((int)(car.currentSpeed * 0.621371)).ToString();
        if (car.currentSpeed >= 0)
        {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, speedFactor);
        }
        else
        {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, -speedFactor);
        }
        pointer.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, -rotationAngle);
        //GUIUtility.RotateAroundPivot(rotationAngle, pivotPoint);
	}
}
