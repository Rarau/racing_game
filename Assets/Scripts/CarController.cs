using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour 
{
    // Input variables for steering the car
    public float throttlePos = 0.0f;
    public float brakePos = 0.0f;
    public float steeringAngle = 0.0f;
    public bool handBrake = false;
    // Car physical constants
    public float cDrag;                             // Drag (air friction constant)
    public float cRoll;                             // Rolling resistance (wheel friction constant)
    public float carMass = 1200.0f;                 // Mass of the car in Kilograms
    public Transform centerOfMass;

    // Car handling variables
    public float brakingPower = 100f;               // Braking power of the car
    public AnimationCurve steeringSensitityCurve;   // How the steering sensitivity changes with the current car speed
    public float steeringSensitivity = 0.5f;        // Multiplier for the steering sensitivity curve
    public float driftAssist = 0.3f;

    // Engine parameters
    public float rpmMin = 1000.0f;                  // Minimum RPM of the engine
    public float rpmMax = 6000.0f;                  // Maximum RPM of the engine
    public float rpm;                               // Current RPM of the engine
    public float engineTorque;                      // Current torque delivered by the engine
    public float maxTorque;                         // Maximum value to clamp the torque
    public float peakTorque = 100f;                 // Value of the torque at the peak of the curve
    public AnimationCurve[] torqueRPMCurve;           // How the torque delivered changes with the RPM
    public float gearRatio = 2.66f;                 // First gear hardcoded
    public float differentialRatio = 3.42f;

    public float maxSpeed;

    public WheelController[] wheels;                // References to the wheel scripts

    // Private variables
    private Rigidbody rigidbody;
    private Vector3 previousVelocity;
    private Vector3 totalAcceleration;
    private Vector3 localVelocity;


    public float ForwardVelocity
    {
        get{ return localVelocity.z; }
    }

    // Car events
    public event Action<int> gearShiftEvent;
    public event Action<Collision> crashEvent;


    public int currentGear;
    public float[] gearsRatio = { -2.769f, 2.083f, 3.769f, 3.267f, 3.538f, 4.083f }; //Toyota Supra
    public int maxGears = 6;
    public float virtualRPM;


    private float timeAccelaration;

    public float currentSpeed = 1;
    public int maximumSpeed = 240; //KM/h

    private bool isGearShiftedDown = false;
    private float timeShift;


    void Start () 
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass.localPosition;

        currentGear = 1; //starting gear, in future we can put a starter

        centerOfMass = transform.FindChild("CenterOfMass");
    }

    void Update()
    {
        timeAccelaration = Mathf.Clamp(timeAccelaration, 0f, timeAccelaration);
        currentSpeed = rigidbody.velocity.magnitude * 3.6f;

        localVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);



        // Get the wheel average rotation rate from the front wheels
        float wheelRotRate = 0.5f * (wheels[2].rpm + wheels[3].rpm);

        // Update the engine RPM from the wheel rotation rate
        rpm = wheelRotRate * gearsRatio[currentGear] * differentialRatio;
        rpm = Mathf.Clamp(rpm, rpmMin, rpmMax);

        // Get the maximum torque the engine can deliver for the current RPM
        maxTorque = GetMaxTorque(rpm);
        // Get the final delivered torque from the throttle position
        engineTorque = maxTorque * throttlePos;
        //if (throttlePos == 0.0f)
        //{
        //    engineTorque = 0.0f;
        //    if (Mathf.Abs(localVelocity.z) < 0.1f)
        //    {
        //        for (int i = 0; i < wheels.Length; i++)
        //        {
        //            if(wheels[i].IsGrounded)
        //                wheels[i].rigidbody.drag = 1000.0f;
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < wheels.Length; i++)
        //    {
        //        wheels[i].rigidbody.drag = 0.0f;
        //    }
        //}



        // Apply the torque to the wheels
        wheels[0].eBrakeEnabled = handBrake;
        wheels[1].eBrakeEnabled = handBrake;
        wheels[2].eBrakeEnabled = handBrake;
        wheels[3].eBrakeEnabled = handBrake;

        wheels[0].brakeTorque = brakingPower * Mathf.Abs(brakePos);
        wheels[1].brakeTorque = brakingPower * Mathf.Abs(brakePos);
        wheels[2].brakeTorque = brakingPower * Mathf.Abs(brakePos);
        wheels[3].brakeTorque = brakingPower * Mathf.Abs(brakePos);

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
        float val = torqueRPMCurve[currentGear].Evaluate(Mathf.Abs(normalizedRPM)) * Mathf.Sign(normalizedRPM);
        return val * peakTorque;
    }


	void FixedUpdate ()
    {
        //float maxSteeringAngle = steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;
        //float currentSteeringAngle = steeringAngle * steeringSensitivity;// * steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;

        //currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -maxSteeringAngle, maxSteeringAngle);

        float currentSteeringAngle = steeringAngle * steeringSensitivity * steeringSensitityCurve.Evaluate(transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed) * 45.0f;
        if (Math.Abs(localVelocity.z) < Math.Abs(localVelocity.x) && localVelocity.magnitude > 1.0f)
        {
            Debug.Log("Drift");
            currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, currentSteeringAngle*Mathf.Abs(wheels[0].slipAngle) * driftAssist, Time.deltaTime * 10.0f);
        }
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
        rigidbody.centerOfMass = Vector3.Lerp(rigidbody.centerOfMass, centerOfMass.localPosition, 0.5f * Time.deltaTime);
        totalAcceleration = (rigidbody.velocity - previousVelocity) / Time.deltaTime;
        totalAcceleration = totalAcceleration.magnitude > 15.0f ? totalAcceleration.normalized * 15.0f : totalAcceleration;
        if (!IsFlying())
        {
            Vector3 v = rigidbody.centerOfMass;
            v -= transform.InverseTransformDirection(Vector3.Scale(totalAcceleration, Vector3.forward + Vector3.right) * 0.0001f);
            v.x = Mathf.Clamp(v.x, -0.45f, 0.45f);
            v.z = Mathf.Clamp(v.z, -0.65f, 0.65f);
            v.y = centerOfMass.localPosition.y;
            //Vector3.ClampMagnitude(v, 0.015f);
            //rigidbody.centerOfMass -= transform.InverseTransformDirection(Vector3.Scale(totalAcceleration, Vector3.forward + Vector3.right) * 0.01f);
            rigidbody.centerOfMass = v;
        }
        //else
        //{
        //    transform.forward = Vector3.Lerp(transform.forward, rigidbody.velocity, Time.deltaTime * 0.80f);

        //}
        previousVelocity = rigidbody.velocity;

        
        if (Mathf.Abs(rigidbody.angularVelocity.y) > 5.0f)
        {
            rigidbody.angularDrag = 3.0f;
        }
        else
        {
            rigidbody.angularDrag = 0.1f;
        }

        foreach (WheelController wc in wheels)
        {
            if (!wc.IsGrounded)
            {
                rigidbody.AddForce(Physics.gravity * 65.0f);
                break;
            }
        }
	}

    public bool IsFlying()
    {
        foreach(WheelController wc in wheels)
        {
            if (wc.IsGrounded)
                return false;
        }
        return true;
    }
    
    

    /// <summary>
    /// Shifts gear up or down according to speed.
    /// </summary>
    void GearsShift()
    {
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(localVelocity.z * 3.6f / maximumSpeed));
        float gearMaxRpm = 1.0f / maxGears;


        float rmpPerGear = 100 * maximumSpeed / maxGears;
        if (currentSpeed > maximumSpeed / maxGears * (currentGear - 1) && currentSpeed < maximumSpeed / maxGears * (currentGear))
        {
            //virtualRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear));
            virtualRPM = normalizedSpeed / (gearMaxRpm * currentGear);
        }
        if (currentGear < maxGears && normalizedSpeed > gearMaxRpm  * currentGear) // && currentSpeed > maximumSpeed / maxGears * (currentGear))
        {
            currentGear++;
            // Fire the gear shift event
            if(gearShiftEvent != null)
            {
                gearShiftEvent(currentGear);
            }
        }
        else if (currentGear > 1 && normalizedSpeed < gearMaxRpm * (currentGear - 1) && !isGearShiftedDown)//currentSpeed < maximumSpeed / maxGears * (currentGear - 1) && !isGearShiftedDown)
        {
            currentGear--;
            // Fire the gear shift event
            if (gearShiftEvent != null)
            {
                gearShiftEvent(currentGear);
            }

            timeShift = Time.timeSinceLevelLoad + 1.0f;
            isGearShiftedDown = true;
        }
        //virtualRPM = (currentSpeed / (maximumSpeed / maxGears * currentGear)) * rpmMax / 1;
        virtualRPM = normalizedSpeed / (gearMaxRpm * currentGear);

        if (Time.timeSinceLevelLoad >= timeShift && isGearShiftedDown)
        {
            isGearShiftedDown = false;
        }
    }


    /// <summary>
    /// Collission handler
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        wheels[2].driveTorque = 0.0f;
        wheels[3].driveTorque = 0.0f;

        if (crashEvent != null)
        {
            crashEvent(other);
        }
    }

    Rect areagui = new Rect(0f, 20f, 500f, 300f);
    bool showDebug;
    void OnGUI()
    {
        GUI.contentColor = Color.black;

        if (GUILayout.Button("Toggle Debug"))
            showDebug = !showDebug;
        if (!showDebug)
            return;
        //GUILayout.BeginArea(areagui, EditorStyles.helpBox);
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
