using UnityEngine;
using System.Collections;

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

    public float gearRatio = 2.66f; // First gear hardcoded
    public float differentialRatio = 3.42f;

	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
    void Update()
    {
        accel = Input.GetKey(KeyCode.W);
        float throttlePos = Input.GetAxis("Vertical");

        float wheelRotRate = wheels[0].AngularVelocity;
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
        float val = torqueRPMCurve.Evaluate(normalizedRPM);

        return val * peakTorque;
    }
	// Update is called once per frame
	void FixedUpdate ()
    {



        Vector3 velocity = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
       // Vector3 fTraction = transform.forward * (accel ? engineForce : 0.0f);
        Vector3 fDrag = -cDrag * rigidbody.velocity;//velocity.z * transform.forward;
        Vector3 fRoll = -cRoll * velocity.z * transform.forward;

        Vector3 fLong = fDrag + fRoll;// +fTraction;  // Total longitudinal force

        Vector3 acceleration = fLong / carMass;

        rigidbody.velocity += acceleration * Time.deltaTime;

	}




    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
