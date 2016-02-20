using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class StraightLineTest : MonoBehaviour {

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

	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
    void Update()
    {
        accel = Input.GetKey(KeyCode.W);
        float throttlePos = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Brakes");
            wheels[0].brakeTorque = brakingPower;
            wheels[1].brakeTorque = brakingPower;
            wheels[2].brakeTorque = brakingPower;
            wheels[3].brakeTorque = brakingPower;

        }
        else
        {
            wheels[0].brakeTorque = 0.0f;
            wheels[1].brakeTorque = 0.0f;
            wheels[2].brakeTorque = 0.0f;
            wheels[3].brakeTorque = 0.0f;
        }
        float wheelRotRate = 0.5f * (wheels[0].AngularVelocity + wheels[1].AngularVelocity);

        rpm = wheelRotRate * gearRatio * differentialRatio * 60.0f / (2.0f * Mathf.PI);
        rpm = Mathf.Clamp(rpm, rpmMin, rpmMax);

        maxTorque = GetMaxTorque(rpm);
        engineTorque = maxTorque * throttlePos;

        wheels[0].driveTorque = engineTorque;
        wheels[1].driveTorque = engineTorque;
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


    Rect areagui = new Rect(0f, 0f, 500f, 300f);
    void OnGUI()
    {
        
        GUILayout.BeginArea(areagui, EditorStyles.helpBox);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Wheel");
        GUILayout.Label("RPM");
        GUILayout.Label("FroceFWD");
        GUILayout.Label("ForceSide");
        GUILayout.Label("SlipRatio");
        GUILayout.Label("SlipAngle");
        GUILayout.EndVertical();

        foreach(WheelController w in wheels)
        {
            GUILayout.BeginVertical();
            GUILayout.Label(w.name);
            GUILayout.Label(w.rpm.ToString("0.0"));
            GUILayout.Label(w.fwdForce.ToString("0.0"));
            GUILayout.Label(w.sideForce.ToString("0.0"));
            GUILayout.Label(w.slipRatio.ToString("0.0"));
            GUILayout.Label((w.slipAngle).ToString("0.0"));
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
