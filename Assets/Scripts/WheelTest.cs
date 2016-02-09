using UnityEngine;
using System.Collections;

public class WheelTest : MonoBehaviour
{
    public float mass = 1.0f;
    public float radius = 0.31f;
    public float motorTorque;
    public float cSpring = 0.1f;
    public float cDamping = 0.5f;

    public Rigidbody rigidbody;

    public Vector3 vel;
    public Vector3 prevPos;

    public float steeringAngle;
    public float slipAngle;

    public float cCorneringStiffnes = 3.0f;
    public float wheelPosY;
    public bool isGrounded = false;

    public WheelFrictionCurve sideFrictionCurve;
    public float sideExtremium;
    public float sideExtremiumValue;
    public float sideAsymptote;
    public float sideAsymptoteValue;
    public float sideStiffness;

    public Vector3 w;

    public float rotationRate;  // RPM

    public float driveTorque;
    public float tractionTorque;
    public float brakeTorque;
    public float totalTorque;
    public float tractionForce;

    public float slipRatio;

    public float angularVelocity = 0.0f;

    public float cTraction = 30.0f;
    public float maxTraction = 500.0f;

    public float linearVel;
    public float vLong;

    public AnimationCurve slipFriction;
    public float wheelAngularAccel;
    public float RotationRate
    {
        get
        {
            return
                //rigidbody.velocity.magnitude / (radius); 
                rotationRate;
        }
        //set { rotationRate = value; }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        UpdateSuspension();



        vLocal = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);

        //rotationRate = rigidbody.velocity.magnitude / (2 * Mathf.PI * radius); 
        //UpdateSlipAngle();
       // angularVelocity = (vLocal.z / radius) * 2.0f * Mathf.PI;
        prevPos = transform.position;

        vLong = vLocal.z;
        //if (Mathf.Abs(vLocal.z) > 0.000f)
        {
            slipRatio = (linearVel - vLocal.z) / vLocal.z;
        }
        slipRatio = Mathf.Clamp(slipRatio, -1.0f, 1.0f);
        if (float.IsNaN(slipRatio) || float.IsInfinity(slipRatio))
            slipRatio = 1.0f * Mathf.Sign(slipRatio);

        tractionForce = slipFriction.Evaluate(Mathf.Abs(slipRatio)) * maxTraction;

        //tractionForce = Mathf.Clamp(tractionForce, -maxTraction, maxTraction);
        tractionTorque = -tractionForce * radius;  //*(- 1.0f * Mathf.Sign(driveTorque));

        totalTorque = driveTorque + tractionTorque + brakeTorque;

        float wheelInertia =  radius * radius * 0.5f;    // Mass is 70.0kg
        wheelAngularAccel = totalTorque / wheelInertia;

        angularVelocity += wheelAngularAccel * Time.deltaTime;
        linearVel = angularVelocity * radius * 0.68f * 0.017453292519968f;
        
        rotationRate = 60.0f * (angularVelocity / (2.0f * Mathf.PI));

       // if (isGrounded)
        {
            rigidbody.AddForceAtPosition(transform.forward * tractionTorque * radius, transform.position);
        }
        
	}

    void UpdateSuspension()
    {
        // This is just suspension
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(transform.position, -transform.up * radius, Color.red);

        vel = (transform.position - prevPos) / Time.deltaTime;
        vel = Vector3.Project(vel, transform.up);

        wheelPosY = 0.0f;
        isGrounded = Physics.Raycast(ray, out hit, radius);
        if (isGrounded)
        {
            //Debug.Log(hit.collider.name);
            rigidbody.AddForceAtPosition(transform.up * -(hit.distance - radius) * cSpring - cDamping * vel, transform.position);
            wheelPosY = hit.distance - radius;
        }

    }

    public Vector3 vLocal;
    void UpdateSlipAngle()
    {
        vLocal = rigidbody.transform.InverseTransformDirection(rigidbody.velocity);

        Vector3 vLateral = Vector3.Project(rigidbody.velocity, rigidbody.transform.right);
        Vector3 vLongitudinal = Vector3.Project(rigidbody.velocity, rigidbody.transform.forward);

        float b = (rigidbody.worldCenterOfMass - transform.position).magnitude;

        float omega = rigidbody.angularVelocity.y;
        w = rigidbody.angularVelocity;

        Vector3 rbVel = rigidbody.transform.forward;
        rbVel.y = 0.0f;
        slipAngle = Vector3.Angle(rbVel, rigidbody.transform.forward);//Mathf.Atan2((vLocal.x + b*omega), vLocal.z) - steeringAngle * Mathf.Sign(vLocal.z) *vLocal.z;

        Debug.Log(Mathf.Atan2((vLocal.x + b * omega), vLocal.z));
       // if (float.IsNaN(slipAngle))
        //    return;


        Vector3 f = rigidbody.transform.right * (slipAngle * cCorneringStiffnes * Mathf.Sign(vLocal.x) * omega * b- steeringAngle);

       //if(isGrounded)
        rigidbody.AddForceAtPosition(f, transform.position);

       Debug.DrawLine(transform.position, transform.position + f, Color.red);
    }

    void UpdateSideFriction()
    {
        sideFrictionCurve.asymptoteSlip = sideAsymptote;
        sideFrictionCurve.asymptoteValue = sideAsymptoteValue;
        sideFrictionCurve.extremumSlip = sideExtremium;
        sideFrictionCurve.extremumValue = sideExtremiumValue;
        sideFrictionCurve.stiffness = sideStiffness;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
