using UnityEngine;
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

    public string playerPrefix = "P1_";


    #region goncalo variables
    public float delayAcellaration;
    public float kilometerPerHour;

    public AnimationCurve[] torqueArrayRPMCurve;

    public int currentGear;
    public float[] gearsRatio = { -2.769f, 2.083f, 3.769f, 3.267f, 3.538f, 4.083f }; //Toyota Supra

    public float timeAccelaration;

    public float currentSpeed = 1;
    public float myCurrentRPM;
    public int maxGears = 6;
    public int maximumSpeed = 240; //KM/h

    public bool isGearShiftedDown = false;
    public float timeShift;
    public GameObject exhaust;
    public float timeCrash;
    public GameObject crash;

    public bool isAccident = false;
    #endregion

    #region speedMeter
    public Texture speedOMeterDial;
    public Texture speedOMeterPointer;

    public float minAnglePointer = 80.0f;
    public float maxAnglePointer = 360.0f;
    #endregion

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        currentGear = 1; //starting gear, in future we can put a starter

        crash = GameObject.Find("CrashParticles");
        if (crash == null)
            Debug.Log("You do not have Crash Particles system!");
        crash.SetActive(false);

        exhaust = GameObject.Find("FireBall");
        if (exhaust == null)
            Debug.Log("You do not have an exhaust system!");
        exhaust.SetActive(false);
    }

    public float engineShaftInertia = 2.0f;

    void Update()
    {

        //accel = Input.GetKey(KeyCode.W);
        timeAccelaration = Mathf.Clamp(timeAccelaration, 0f, timeAccelaration);
        currentSpeed = rigidbody.velocity.magnitude * 3.6f;

        // just commented to ee behaviour with reserve gear
        //if ( Input.GetAxis(playerPrefix + "Vertical") < 0.0f)
        //{
        //    Debug.Log("Brakes");
        //    wheels[0].brakeTorque = brakingPower;
        //    wheels[1].brakeTorque = brakingPower;
        //    wheels[2].brakeTorque = brakingPower;
        //    wheels[3].brakeTorque = brakingPower;     
        //}
        //else
        //{
        //    throttlePos = Input.GetAxis(playerPrefix + "Vertical");

        //    wheels[0].brakeTorque = 0.0f;
        //    wheels[1].brakeTorque = 0.0f;
        //    wheels[2].brakeTorque = 0.0f;
        //    wheels[3].brakeTorque = 0.0f;
        //}

        if (Input.GetAxis(playerPrefix + "Vertical") > -1.0f) {
            throttlePos = Input.GetAxis(playerPrefix + "Vertical");
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

        // Manual gears
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    currentGear = Mathf.Clamp(currentGear + 1, 1, gearsRatio.Length);
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    currentGear = Mathf.Clamp(currentGear - 1, 1, gearsRatio.Length);

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
        steeringAngle = Input.GetAxis(playerPrefix + "Horizontal") * steeringSensitivity * steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;
        steeringAngle = Mathf.Clamp(steeringAngle, -45.0f, 45.0f);
        //wheels[0].transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 30.0f - transform.rotation.y, Space.Self);
        //wheels[1].transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 30.0f - transform.rotation.y, Space.Self);
        //wheels[0].transform.localRotation = Quaternion.Euler(0.0f, steeringAngle, 0.0f);
        //wheels[1].transform.localRotation = Quaternion.Euler(0.0f, steeringAngle, 0.0f);

        wheels[0].steeringAngle = steeringAngle;
        wheels[1].steeringAngle = steeringAngle;

        //wheels[0].overrideSlipRatio = true;
        //wheels[0].overridenSlipRatio = wheels[1].slipRatio;

        //wheels[3].overrideSlipRatio = true;
        //wheels[3].overridenSlipRatio = wheels[2].slipRatio;

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


    /// <summary>
    /// Shifts gear up or down according to speed.
    /// </summary>
    void GearsShift()
    {
        if (currentSpeed > maximumSpeed / maxGears * (currentGear - 1) && currentSpeed < maximumSpeed / maxGears * (currentGear))
        {
            myCurrentRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear)) * rpmMax / 10;
        }
        if (currentGear < maxGears && currentSpeed > maximumSpeed / maxGears * (currentGear))
        {
            currentGear++;
        }
        else if (currentGear > 1 && currentSpeed < maximumSpeed / maxGears * (currentGear - 1) && !isGearShiftedDown)
        {
            currentGear--;
            // fireBall goes here
            timeShift = Time.timeSinceLevelLoad + 1.0f;
            isGearShiftedDown = true;
            if (currentSpeed > 40)
                exhaust.SetActive(true);
        }
        myCurrentRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear)) * rpmMax / 1;
        
        if (Time.timeSinceLevelLoad >= timeShift && isGearShiftedDown)
        {
            isGearShiftedDown = false;
            exhaust.SetActive(false);
        }

    }


    ////Speedometer

    //function OnGUI()
    //{
    //    GUI.DrawTexture(Rect(Screen.width - 300, Screen.height - 300, 300, 300), speedOMeterDial);
    //    var speedFactor : float = currentSpeed / topSpeed;
    //    var rotationAngle : float;
    //    if (currentSpeed >= 0)
    //    {
    //        rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, speedFactor);
    //    }
    //    else {
    //        rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, -speedFactor);
    //    }
    //    GUIUtility.RotateAroundPivot(rotationAngle, Vector2(Screen.width - 150, Screen.height - 150));
    //    GUI.DrawTexture(Rect(Screen.width - 300, Screen.height - 300, 300, 300), speedOMeterPointer);
    //}


    /// <summary>
    /// Collission handler
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        crash.SetActive(true);

        timeCrash = Time.timeSinceLevelLoad + 1.0f;

        isAccident = true;
    }

    /// <summary>
    /// Collission handler
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionExit(Collision other)
    {
        //if (Time.timeSinceLevelLoad >= timeCrash)
            crash.SetActive(false);

        isAccident = false;
    }

    Rect areagui = new Rect(0f, 20f, 500f, 300f);
    bool showDebug;
    void OnGUI()
    {
        GUI.contentColor = Color.black;
        Rect rect = new Rect(Screen.width - 100, Screen.height - 100, 100, 100);
        GUI.DrawTexture(rect, speedOMeterDial);
        float speedFactor = currentSpeed / maxSpeed;
        float rpmFactor = myCurrentRPM / rpmMax;
        float rotationAngle;
        if (currentSpeed >= 0)
        {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, rpmFactor);
        }
        else {
            rotationAngle = Mathf.Lerp(minAnglePointer, maxAnglePointer, -rpmFactor);
        }
        //Vector2 pivotPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 pivotPoint = new Vector2(Screen.width-50, Screen.height-50);
        GUIUtility.RotateAroundPivot(rotationAngle, pivotPoint);
        GUI.DrawTexture(rect, speedOMeterPointer);

        //if (GUILayout.Button("Toggle Debug"))
        //    showDebug = !showDebug;
        //if (!showDebug)
        //    return;
        //GUILayout.BeginArea(areagui, EditorStyles.helpBox);
        //GUILayout.BeginHorizontal();

        //GUILayout.BeginVertical();
        //GUILayout.Label("Wheel");
        //GUILayout.Label("RPM");
        //GUILayout.Label("FroceFWD");
        //GUILayout.Label("ForceSide");
        //GUILayout.Label("SlipRatio");
        //GUILayout.Label("SlipAngle");
        //GUILayout.Label("LinearVel");

        //GUILayout.EndVertical();

        //foreach(WheelController w in wheels)
        //{
        //    GUILayout.BeginVertical();
        //    GUILayout.Label(w.name);
        //    GUILayout.Label(w.rpm.ToString("0.0"));
        //    GUILayout.Label(w.fwdForce.ToString("0.0"));
        //    GUILayout.Label(w.sideForce.ToString("0.0"));
        //    GUILayout.Label(w.slipRatio.ToString("0.000"));
        //    GUILayout.Label((w.slipAngle).ToString("0.0"));
        //    GUILayout.Label((w.linearVel).ToString("0.00"));

        //    GUILayout.EndVertical();
        //}
        //GUILayout.EndHorizontal();
        //GUILayout.EndArea();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
