﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour {

    //public float engineForce = 10.0f;   // Constant force from the engine
    public float cDrag;                 // Drag (air friction constant)
    public float cRoll;                 // Rolling resistance (wheel friction constant)

    public float carMass = 1200.0f;     // Mass of the car in Kilograms

    Rigidbody rigidbody;

    bool accel;

    //public WheelTest[] wheels;
    public WheelController[] wheels;

    //Engine parameters:
    public float rpmMin = 1000.0f;
    public float rpmMax = 6000.0f;
    public float rpm;
    public float engineTorque;
    public float throttlePos;
    public float maxTorque;
    public float peakTorque  = 100f;
    public AnimationCurve torqueRPMCurve;
    public Transform centerOfMass;
    public float gearRatio = 2.66f; // First gear hardcoded
    public float differentialRatio = 3.42f;
    public float steeringSensitivity = 0.5f;

    public AnimationCurve steeringSensitityCurve;
    public float maxSpeed;

    public float brakingPower = 100f;


    #region goncalo variables
    public float delayAcellaration;
    public float kilometerPerHour;

    public AnimationCurve[] torqueArrayRPMCurve;

    public int currentGear;
    public float[] gearsRatio = { -2.769f, 2.083f, 3.769f, 3.267f, 3.538f, 4.083f }; //Toyota Supra

    public float timeAccelaration;

    public float currentSpeed;
    public float myCurrentRPM;
    public int maxGears = 5;
    public int maximumSpeed = 200; //KM/h

    public bool isGearShiftedDown = false;
    public float timeShift;

    public GameObject exhaust;
    #endregion

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        currentGear = 1; //starting gear, in future we can put a starter

        exhaust = GameObject.Find("FireBall");
        if (exhaust == null)
            Debug.Log("You do not have an exhaust system!");
        exhaust.SetActive(false);
    }

    public float engineShaftInertia = 2.0f;

    void Update()
    {

       // accel = Input.GetKey(KeyCode.W);
        timeAccelaration = Mathf.Clamp(timeAccelaration, 0f, timeAccelaration);
        currentSpeed = rigidbody.velocity.magnitude * 3.6f;

        if ( Input.GetAxis("Vertical") < 0.0f)
        {
            Debug.Log("Brakes");
            wheels[0].brakeTorque = brakingPower;
            wheels[1].brakeTorque = brakingPower;
            wheels[2].brakeTorque = brakingPower;
            wheels[3].brakeTorque = brakingPower;

        }
        else
        {
            throttlePos = Input.GetAxis("Vertical");

            wheels[0].brakeTorque = 0.0f;
            wheels[1].brakeTorque = 0.0f;
            wheels[2].brakeTorque = 0.0f;
            wheels[3].brakeTorque = 0.0f;
        }
        float wheelRotRate = 0.5f * (wheels[0].rpm + wheels[1].rpm);

        // If we are not accelerating (no throttle) we slow down the engine
        if (throttlePos == 0.0f)
            rpm -= Time.deltaTime * engineShaftInertia;
        else
        {
            rpm = Mathf.Lerp(rpm, wheelRotRate * gearsRatio[currentGear] * differentialRatio, Time.deltaTime * 3500f);// *60.0f / (2.0f * Mathf.PI);
        }
        rpm = Mathf.Clamp(rpm, rpmMin, rpmMax);

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    currentGear = Mathf.Clamp(currentGear + 1, 1, gearsRatio.Length);
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    currentGear = Mathf.Clamp(currentGear - 1, 1, gearsRatio.Length);

        //ShiftGears();

        maxTorque = GetMaxTorque(rpm);
        engineTorque = maxTorque * throttlePos;
        if (throttlePos == 0.0f)
            engineTorque = 0.0f;
        //wheels[1].driveTorque = engineTorque;
        //wheels[0].driveTorque = engineTorque;
        wheels[2].driveTorque = engineTorque;
        wheels[3].driveTorque = engineTorque;

        GearsShift();

    }


    float lastThrottleTime;
    //float throttlePos;
    public void PushThrottle()
    {
        lastThrottleTime = Time.time;

    }

    public void ReleaseThrottle()
    {
        throttlePos = 0.0f;
    }

    public float normalizedRPM;
    float GetMaxTorque(float currentRPM)
    {
        normalizedRPM = (currentRPM - rpmMin) / (rpmMax - rpmMin);
        float val = torqueRPMCurve.Evaluate(Mathf.Abs(normalizedRPM)) * Mathf.Sign(normalizedRPM);

        return val * peakTorque;
    }

    Vector3 prevVel, totalAccel;
    float steeringAngle = 0.0f;
    public float carAngularSpeed = 0f;
	// Update is called once per frame
	void FixedUpdate ()
    {
        rigidbody.centerOfMass = centerOfMass.localPosition;
        carAngularSpeed = rigidbody.angularVelocity.y;
        steeringAngle = Input.GetAxis("Horizontal") * steeringSensitivity * steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;
        steeringAngle = Mathf.Clamp(steeringAngle, -45.0f, 45.0f);
        //wheels[0].transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 30.0f - transform.rotation.y, Space.Self);
        //wheels[1].transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 30.0f - transform.rotation.y, Space.Self);
        //wheels[0].transform.localRotation = Quaternion.Euler(0.0f, steeringAngle, 0.0f);
        //wheels[1].transform.localRotation = Quaternion.Euler(0.0f, steeringAngle, 0.0f);

        wheels[0].steeringAngle = steeringAngle;
        wheels[1].steeringAngle = steeringAngle;

        //wheels[0].overrideSlipRatio = true;
        //wheels[0].overridenSlipRatio = wheels[1].slipRatio;

        wheels[2].overrideSlipRatio = true;
        wheels[2].overridenSlipRatio = wheels[3].slipRatio;

        Vector3 velocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
       // Vector3 fTraction = transform.forward * (accel ? engineForce : 0.0f);
        Vector3 fDrag = -cDrag * velocity.z * velocity.z * transform.forward;
        Vector3 fRoll = -cRoll * velocity.z * transform.forward;

        Vector3 fLong = fDrag + fRoll;// +fTraction;  // Total longitudinal force

        Vector3 acceleration = fLong / carMass;
        Debug.DrawLine(transform.position, transform.position + totalAccel, Color.magenta);
       // Debug.DrawLine(transform.position, transform.position + rigidbody.velocity, Color.cyan);

        rigidbody.velocity += acceleration * Time.deltaTime;//rigidbody.transform.TransformDirection( acceleration) * Time.deltaTime;
        //rigidbody.AddForce( rigidbody.transform.TransformDirection(fLong));

        totalAccel = (rigidbody.velocity - prevVel) / Time.deltaTime;
        totalAccel = totalAccel.magnitude > 15.0f ? totalAccel.normalized : totalAccel;
        rigidbody.centerOfMass -= transform.InverseTransformDirection(Vector3.Scale(totalAccel, Vector3.forward + Vector3.right) * 0.01f);
        //rigidbody.angularDrag = rigidbody.angularVelocity.y * 0.1f;
        prevVel = rigidbody.velocity;
        if (Mathf.Abs(rigidbody.angularVelocity.y) > 5.0f)
            rigidbody.angularDrag = 3.0f;
        else
            rigidbody.angularDrag = 0.1f;


	}

    void GearsShift()
    {
        if (currentSpeed > maximumSpeed / maxGears * (currentGear - 1) && currentSpeed < maximumSpeed / maxGears * (currentGear))
        {
            //myCurrentRPM = (currentSpeed) / (maximumSpeed / maxGears * currentSpeed) * rpmMax;
            myCurrentRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear)) * rpmMax / 10;
        }
        if (currentGear < maxGears && currentSpeed > maximumSpeed / maxGears * (currentGear))
        {
            currentGear++;
            // myCurrentRPM = rpmMin;
        }
        else if (currentGear > 1 && currentSpeed < maximumSpeed / maxGears * (currentGear - 1) && !isGearShiftedDown)
        {
            currentGear--;
            // fireBall goes here
            timeShift = Time.timeSinceLevelLoad + 1.0f;
            isGearShiftedDown = true;
            exhaust.SetActive(true);
        }
        //Debug.Log("currentSpeed"+ currentSpeed+" ** Max Speed Current Gear" + ((maximumSpeed / maxGears * currentGear)));
        myCurrentRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear)) * rpmMax / 10;

        //Debug.Log("\n scurrent time: " + Time.timeSinceLevelLoad);
        //Debug.Log("\n stimeShift: " + timeShift);
        
        if (Time.timeSinceLevelLoad >= timeShift && isGearShiftedDown)
        {
            isGearShiftedDown = false;
            exhaust.SetActive(false);
        }

    }


    void ShiftGears()
    {
        float acumRPM = 0;

        //float kilometerPerHour = rigidbody.velocity.magnitude * 3.6f;

        //if (kilometerPerHour > -1 && kilometerPerHour < 50) currentGear = 1;
        //if (kilometerPerHour >= 50 && kilometerPerHour < 80) currentGear = 2;
        //if (kilometerPerHour >= 80 && kilometerPerHour < 100) currentGear = 3;
        //if (kilometerPerHour >= 100 && kilometerPerHour < 130) currentGear = 4;
        //if (kilometerPerHour >= 130 && kilometerPerHour < 250) currentGear = 5;

        kilometerPerHour = (rigidbody.velocity.magnitude * 3.6f) / 140;
        if (kilometerPerHour < 1)
            kilometerPerHour = 1;

        if (Input.GetKey(KeyCode.W))
            timeAccelaration = (timeAccelaration + 1) * kilometerPerHour;
        else
            timeAccelaration = (timeAccelaration - 2) * kilometerPerHour;
        timeAccelaration = Mathf.Clamp(timeAccelaration, 0f, 140f);

        if (timeAccelaration >= 140 && accel && currentGear < gearsRatio.Length - 1)
        {
            timeAccelaration = 0;
            currentGear++;
        }
        if (timeAccelaration < 70 && !accel && currentGear > 1)
        {
            timeAccelaration = 120;
            currentGear--;
        }
        //change gear up
        //if (currentRPM >= rpmMax && currentGear < gearsRatio.Length - 1 && accel)
        //{
        //    for (var j = 1; j <= currentGear; j++)
        //    {
        //        acumRPM += currentRPM * gearsRatio[j];
        //        if (acumRPM >= gearsRatio[j]*j*rpmMax)
        //        {
        //            currentGear++;
        //            break;
        //        }
        //    }
        //    currentRPM = rpmMin;
        //}

        ////change gear down
        //if (currentRPM <= (rpmMax - rpmMin) && !accel && currentGear > 1)
        //{
        //    for (var j = currentGear; j >= 1; j--)
        //    {
        //        acumRPM -= currentRPM * gearsRatio[j];
        //        //Debug.Log(acumRPM);
        //        if (acumRPM <= gearsRatio[j] * (j-1) * rpmMax)
        //        {
        //            currentGear--;
        //            break;
        //        }
        //    }
        //    currentRPM = rpmMax;
        //}

        //currentRPM = Mathf.Clamp(currentRPM, rpmMin, rpmMax);
    }


    Rect areagui = new Rect(0f, 20f, 500f, 300f);
    bool showDebug;
    void OnGUI()
    {
        if (GUILayout.Button("Toggle Debug"))
            showDebug = !showDebug;
        if (!showDebug)
            return;
        GUILayout.BeginArea(areagui, EditorStyles.helpBox);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Wheel");
        GUILayout.Label("RPM");
        GUILayout.Label("FroceFWD");
        GUILayout.Label("ForceSide");
        GUILayout.Label("SlipRatio");
        GUILayout.Label("SlipAngle");
        GUILayout.Label("LinearVel");

        GUILayout.EndVertical();

        foreach(WheelController w in wheels)
        {
            GUILayout.BeginVertical();
            GUILayout.Label(w.name);
            GUILayout.Label(w.rpm.ToString("0.0"));
            GUILayout.Label(w.fwdForce.ToString("0.0"));
            GUILayout.Label(w.sideForce.ToString("0.0"));
            GUILayout.Label(w.slipRatio.ToString("0.000"));
            GUILayout.Label((w.slipAngle).ToString("0.0"));
            GUILayout.Label((w.linearVel).ToString("0.00"));

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
