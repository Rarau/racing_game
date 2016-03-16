using UnityEngine;
using System.Collections;

public class WheelController : MonoBehaviour { 
    public Rigidbody rigidbody;
    // In Kilograms
    public float mass = 30.0f;
    // In meters
    public float radius = 0.5f;
    public float tractionCoeff = 8.0f;
    public float maxTractionAmt = 100.0f;
    public float sideTraction;

    // In degrees per second
    public float angularVelocityDegSec;

    // In radians per second
    public float AngularVelocity
    {
        get { return angularVelocityDegSec * 0.017453292519968f; }
        set { angularVelocityDegSec = value; }
    }

    public float rpm
    {
        get { return AngularVelocity / ((2.0f * Mathf.PI)/60.0f); }
    }

    public float driveTorque;
    public float brakeTorque;

    public float tractionTorque;
    public float totalTorque;
    public float linearVel;
    public float slipRatio;

    public Vector3 localVel;

    public float slipAngle;

    public AnimationCurve frictionCurve;
    public AnimationCurve sideCurve;
    public AnimationCurve forceCurve;

    public GameObject wheelGeometry;
    public float tractionForce;
    public float maxSideForce = 500.0f;

    public float distanceToCM;
    public LayerMask raycastIgnore;
    public float steeringAngle;
    public Vector3 fwd;

    public bool overrideSlipRatio;
    public float overridenSlipRatio;
    float prevSteringAngle;
    public bool eBrakeEnabled = false;
    public WheelController connectedWheel;

	// Update is called once per frame
	void Update () 
    {
        rigidbody = GetComponent<Rigidbody>();

        //wheelGeometry.transform.localRotation *= Quaternion.Euler(0.0f, steeringAngle - prevSteringAngle, 0.0f);
        wheelGeometry.transform.Rotate(rigidbody.transform.up, steeringAngle - prevSteringAngle, Space.World);

        wheelGeometry.transform.Rotate(Vector3.right, angularVelocityDegSec * Time.deltaTime, Space.Self);
        wheelGeometry.transform.localPosition = transform.localPosition;
        prevSteringAngle = steeringAngle;
	}
    void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = (Vector3.zero);
    }

    private Vector3 prevNormal;
    RaycastHit groundInfo;
    void FixedUpdate()
    {
        if (overrideSlipRatio)
            slipRatio = overridenSlipRatio;
        if (Physics.Raycast(transform.position, -rigidbody.transform.up, out groundInfo, radius, raycastIgnore))
        {
            prevNormal = groundInfo.normal;
            //Debug.Log(name + ": " + Vector3.Dot(groundInfo.normal, prevNormal));
            SimulateTraction();
        }

    }

    public Vector3 prevPos;
    public float w;

    public float fwdForce;
    public float sideForce;
    public float atanValue;

    public float downForce = 10.0f;

    Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 v )
    {
        planeNormal.Normalize();
        var distance = -Vector3.Dot(planeNormal.normalized, v);
        return v + planeNormal * distance;
    }

    public float weightTransfer;
    void SimulateTraction()
    {
        weightTransfer = 1.9f * 1.0f / Vector3.Distance(transform.position, transform.root.GetComponent<Rigidbody>().worldCenterOfMass);

        fwd = rigidbody.transform.forward;
        fwd = Quaternion.Euler(0, steeringAngle, 0) * fwd;
        fwd.Normalize();
        
        Vector3 right = rigidbody.transform.right;
        right = Vector3.Cross(transform.up, fwd).normalized;
        right = ProjectVectorOnPlane(groundInfo.normal, right);

        //localVel = Vector3.Lerp(localVel, transform.InverseTransformDirection(rigidbody.GetPointVelocity(transform.position)), Time.fixedDeltaTime * 1.5f);
        localVel = transform.InverseTransformDirection(rigidbody.GetPointVelocity(transform.position));
        Debug.DrawLine(transform.position, transform.position + rigidbody.GetPointVelocity(transform.position), Color.green);
        //totalTorque = driveTorque + tractionTorque + brakeTorque;

        float wheelInertia = mass * radius * radius * 0.5f;    // Mass is 70.0kg
        //totalTorque = (-1.0f * Mathf.Sign(driveTorque) * tractionTorque + driveTorque - brakeTorque);
        //Mathf.Clamp(brakeTorque, -driveTorque, driveTorque);
        if (angularVelocityDegSec == 0.0f)
            brakeTorque = 0.0f;
        brakeTorque = -brakeTorque;
        if (linearVel < 0.0f)
            brakeTorque = 0.0f;
       // totalTorque = driveTorque - brakeTorque;
        totalTorque = driveTorque + brakeTorque;

        float wheelAngularAccel = (totalTorque) / wheelInertia;

        // If the wheel is driven by the engine or braking
        if (totalTorque != 0.0f)
        {
            angularVelocityDegSec += wheelAngularAccel * Time.fixedDeltaTime;
            linearVel = angularVelocityDegSec * 0.017453292519968f * radius;
        }
        // If the wheel is spinning free
        else
        {
            angularVelocityDegSec = (localVel.z) * (1.0f / 0.017453292519968f) * (1.0f / radius);
            //linearVel = Mathf.Lerp (linearVel, localVel.z, Time.fixedDeltaTime * 300.0f);
            linearVel = localVel.z;
        }

        if(eBrakeEnabled)
        {
            angularVelocityDegSec = 0.0f;
            linearVel = 0.0f;
        }

        if (!overrideSlipRatio) {
            slipRatio = (linearVel - localVel.z) / Mathf.Abs(localVel.z) * 0.1f;
            slipRatio = Mathf.Clamp(slipRatio, -6f, 6f);
            // If it's NaN, then the car and the wheel are stopped (0 / 0 division)
            if (float.IsNaN(slipRatio)) {
                slipRatio = 0.0f;
            }
            // If it's infinity, then the wheel is spinning but the car is stopped (x / 0) division
            else if (float.IsInfinity(slipRatio)) {
                slipRatio = 1.0f * Mathf.Sign(slipRatio);
            }
        } else {
            slipRatio = connectedWheel.slipRatio;
        }




        tractionForce = frictionCurve.Evaluate(Mathf.Abs(slipRatio)) * tractionCoeff * Mathf.Sign(slipRatio); //* sideCurve.Evaluate(Mathf.Abs(slipAngle / 90.0f));
        tractionForce = Mathf.Clamp(tractionForce, -maxTractionAmt, maxTractionAmt);
        tractionTorque = tractionForce / radius;

        Vector3 tractionForceV = fwd * tractionForce;
        //Debug.DrawLine(transform.position, 0.01f * tractionForceV + transform.position, Color.red);

        //if(Mathf.Abs(slipRatio) > 0.01f)
        if(totalTorque != 0.0f) {
            rigidbody.AddForceAtPosition(tractionForceV * weightTransfer * transform.root.GetComponent<Rigidbody>().mass, transform.position);
            //rigidbody.AddForceAtPosition(fwd * 100.0f, transform.position);
            //Debug.Log(gameObject.name + " " + tractionForce + " " + slipRatio + " " + linearVel + " " + localVel.z);
        }

        fwdForce = tractionForceV.magnitude;


        // Calculate the slip angle: Angle between forward direction vector of the wheel and velocity vector of the wheel

        slipAngle = Vector3.Angle(Mathf.Sign(linearVel) * fwd, rigidbody.velocity.normalized);
        Vector3 cross = Vector3.Cross(Mathf.Sign(linearVel) * fwd, rigidbody.velocity.normalized);

        if (cross.y < 0) slipAngle = -slipAngle;

        slipAngle *= Mathf.Sign(linearVel);

        Vector3 sideForce = -right * sideCurve.Evaluate(Mathf.Abs(slipAngle / 90.0f)) * Mathf.Sign(slipAngle) * sideTraction;// * Mathf.Clamp((Mathf.Abs(localVel.x) / 0.010f), 0.0f, 1.0f);

        float sideForceMultiplier = forceCurve.Evaluate(localVel.magnitude / 2.0f);
        sideForce *= sideForceMultiplier;
        //this.sideForce = sideForce.magnitude;
        sideForce = sideForce.magnitude > maxSideForce ? sideForce.normalized * maxSideForce : sideForce;
        //sideForce *= Mathf.Clamp(rigidbody.velocity.magnitude / 0.50f , - 1.0f, 1.0f);
        //sideForce = transform.TransformDirection(sideForce);


        Debug.DrawLine(transform.position, sideForce + transform.position, Color.yellow);

        prevPos = transform.position;

        Debug.DrawLine(transform.position, transform.position + fwd * 5.0f, Color.blue);
        //Debug.DrawLine(transform.position, transform.position + right * 5.0f, Color.red);
        //if (Mathf.Abs(linearVel) < 0.82f && totalTorque == 0.0f)
        //{
        //    rigidbody.drag = Mathf.Lerp(rigidbody.drag, 100.0f, Time.deltaTime * 10.0f);
        //}
        //else
        //{
        //    rigidbody.drag = Mathf.Lerp(rigidbody.drag, 0.0f, Time.deltaTime * 10.0f);
           rigidbody.AddForceAtPosition(sideForce * weightTransfer * transform.root.GetComponent<Rigidbody>().mass, transform.position);

        //}

       // rigidbody.AddForce(-downForce * rigidbody.transform.up);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(weightTransfer, weightTransfer, 1.0f);

        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
