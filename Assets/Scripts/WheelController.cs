using UnityEngine;
using System.Collections;

public class WheelController : MonoBehaviour { 
    public Rigidbody rigidbody;
    public float mass = 30.0f;
    public float radius = 0.5f;
    public float tractionCoeff = 8.0f;
    public float maxTractionAmt = 100.0f;

    public float angularVelocity;

    public float AngularVelocity
    {
        get { return angularVelocity * 0.017453292519968f; }
        set { angularVelocity = value; }
    }

    public float rpm
    {
        get { return angularVelocity; }
    }

    public float driveTorque;
    public float brakeTorque;

    public float tractionTorque;
    public float totalTorque;
    public float linearVel;
    public float slipRatio;

    public Vector3 localVel;

    public AnimationCurve frictionCurve;

	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime, Space.Self);
	}

    void FixedUpdate()
    {
        SimulateTraction();
    }

    void SimulateTraction()
    {
        // TO-DO: Change this to wheel space
        localVel = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);
        //totalTorque = driveTorque + tractionTorque + brakeTorque;

        float wheelInertia = mass * radius * radius * 0.5f;    // Mass is 70.0kg
        totalTorque = (-1.0f * Mathf.Sign(driveTorque) * tractionTorque + driveTorque - brakeTorque);
        //totalTorque = driveTorque - brakeTorque;
        float wheelAngularAccel = (totalTorque) / wheelInertia;

        // If the wheel is driven by the engine
        if (driveTorque != 0.0f)
            angularVelocity += wheelAngularAccel * Time.fixedDeltaTime;
        // If the wheel is spinning free
        else
            angularVelocity = (localVel.z) * (1.0f / 0.017453292519968f) * (1.0f / radius);

        linearVel = angularVelocity * 0.017453292519968f * radius;

        slipRatio = (linearVel - localVel.z) / Mathf.Abs(localVel.z);

        // If it's NaN, then the car and the wheel are stopped (0 / 0 division)
        if (float.IsNaN(slipRatio))
        {
            slipRatio = 0.0f;
        }
        // If it's infinity, then the wheel is spinning but the car is stopped (x / 0) division
        else if (float.IsInfinity(slipRatio))
        {
            slipRatio = 1.0f * Mathf.Sign(slipRatio);
        }
        


        //transform.position += Vector3.right * linearVel * Time.fixedDeltaTime;


        //float tractionForce = slipRatio * tractionCoeff;
        float tractionForce = frictionCurve.Evaluate(Mathf.Abs(slipRatio)) * tractionCoeff * Mathf.Sign(slipRatio);
        tractionForce = Mathf.Clamp(tractionForce, -maxTractionAmt, maxTractionAmt);
        tractionTorque = tractionForce / radius;

        //if(driveTorque != 0.0f)
            rigidbody.AddForceAtPosition(rigidbody.transform.forward * tractionForce, transform.position);
    }
}
