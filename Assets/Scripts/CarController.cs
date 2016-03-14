using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour 
{
    // Input variables for steering the car
    public float throttlePos = 0.0f;
    public float steeringAngle = 0.0f;
    public string playerPrefix = "P1_";             // Prefix concatenated to the Input axis name for multiplayer support

    // Car physical constants
    public float cDrag;                             // Drag (air friction constant)
    public float cRoll;                             // Rolling resistance (wheel friction constant)
    public float carMass = 1200.0f;                 // Mass of the car in Kilograms
    public Transform centerOfMass;

    // Car handling variables
    public float brakingPower = 100f;               // Braking power of the car
    public AnimationCurve steeringSensitityCurve;   // How the steering sensitivity changes with the current car speed
    public float steeringSensitivity = 0.5f;        // Multiplier for the steering sensitivity curve

    // Engine parameters
    public float rpmMin = 1000.0f;                  // Minimum RPM of the engine
    public float rpmMax = 6000.0f;                  // Maximum RPM of the engine
    public float rpm;                               // Current RPM of the engine
    public float engineTorque;                      // Current torque delivered by the engine
    public float maxTorque;                         // Maximum value to clamp the torque
    public float peakTorque = 100f;                 // Value of the torque at the peak of the curve
    public AnimationCurve torqueRPMCurve;           // How the torque delivered changes with the RPM
    public float gearRatio = 2.66f;                 // First gear hardcoded
    public float differentialRatio = 3.42f;

    public float maxSpeed;

    public WheelController[] wheels;                // References to the wheel scripts

    // Private variables
    private Rigidbody rigidbody;
    private Vector3 previousVelocity;
    private Vector3 totalAcceleration;



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
        /*
        if (Input.GetAxis(playerPrefix + "Vertical") > -1.0f)
        {
            throttlePos = Input.GetAxis(playerPrefix + "Vertical");
        }
        */
        // Get the wheel average rotation rate from the front wheels
        float wheelRotRate = 0.5f * (wheels[0].rpm + wheels[1].rpm);

        // Update the engine RPM from the wheel rotation rate
        rpm = wheelRotRate * gearsRatio[currentGear] * differentialRatio;
        rpm = Mathf.Clamp(rpm, rpmMin, rpmMax);

        // Get the maximum torque the engine can deliver for the current RPM
        maxTorque = GetMaxTorque(rpm);
        // Get the final delivered torque from the throttle position
        engineTorque = maxTorque * throttlePos;
        if (throttlePos == 0.0f)
        {
            engineTorque = 0.0f;
        }

        // Apply the torque to the wheels
        //wheels[1].driveTorque = engineTorque;
        //wheels[0].driveTorque = engineTorque;
        wheels[2].driveTorque = engineTorque;
        wheels[3].driveTorque = engineTorque;

        GearsShift();
    }



    /// <summary>
    /// Sample the RPM vs torque engine curve to get the torque for the current RPM value
    /// </summary>
    /// <param name="currentRPM">The current engine RPM</param>
    float GetMaxTorque(float currentRPM)
    {
        float normalizedRPM = (currentRPM - rpmMin) / (rpmMax - rpmMin);
        float val = torqueRPMCurve.Evaluate(Mathf.Abs(normalizedRPM)) * Mathf.Sign(normalizedRPM);
        return val * peakTorque;
    }


	void FixedUpdate ()
    { 
        float currentSteeringAngle = steeringAngle * steeringSensitivity * steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;
        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -45.0f, 45.0f);

        // Turn the front wheels according to the input
        wheels[0].steeringAngle = currentSteeringAngle;
        wheels[1].steeringAngle = currentSteeringAngle;

        //wheels[0].overrideSlipRatio = true;
        //wheels[0].overridenSlipRatio = wheels[1].slipRatio;

        //wheels[3].overrideSlipRatio = true;
        //wheels[3].overridenSlipRatio = wheels[2].slipRatio;

        // Calculate and apply the longitudinal force (comes from air drag and rolling friction)
        Vector3 velocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
        Vector3 fDrag = -cDrag * velocity.z * velocity.z * transform.forward;
        Vector3 fRoll = -cRoll * velocity.z * transform.forward;
        Vector3 fLong = fDrag + fRoll;  
        Vector3 acceleration = fLong / carMass;
        rigidbody.velocity += acceleration * Time.deltaTime;
        //Debug.DrawLine(transform.position, transform.position + totalAcceleration, Color.magenta);

        // Calculate the total acceleration of the car and use it to displace the center of mass.
        // This way we get different weight transfer to each wheel
        rigidbody.centerOfMass = centerOfMass.localPosition;
        totalAcceleration = (rigidbody.velocity - previousVelocity) / Time.deltaTime;
        totalAcceleration = totalAcceleration.magnitude > 15.0f ? totalAcceleration.normalized : totalAcceleration;
        rigidbody.centerOfMass -= transform.InverseTransformDirection(Vector3.Scale(totalAcceleration, Vector3.forward + Vector3.right) * 0.01f);
        previousVelocity = rigidbody.velocity;

        
        if (Mathf.Abs(rigidbody.angularVelocity.y) > 5.0f)
        {
            rigidbody.angularDrag = 3.0f;
        }
        else
        {
            rigidbody.angularDrag = 0.1f;
        }

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

        foreach (WheelController w in wheels)
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
