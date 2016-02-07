using UnityEngine;
using System.Collections;

public class WheelTest : MonoBehaviour
{
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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(transform.position, -transform.up * radius, Color.red);

        vel = (transform.position - prevPos) / Time.deltaTime;
        vel = Vector3.Project(vel, transform.up);

	    if(Physics.Raycast(ray, out hit, radius))
        {
            Debug.Log(hit.collider.name);
            rigidbody.AddForceAtPosition(transform.up * -(hit.distance - radius) * cSpring - cDamping * vel, transform.position);
        }

        UpdateSlipAngle();

        prevPos = transform.position;
	}

    void UpdateSlipAngle()
    {
        Vector3 vLateral = Vector3.Project(rigidbody.velocity, rigidbody.transform.right);
        Vector3 vLongitudinal = Vector3.Project(rigidbody.velocity, rigidbody.transform.forward);


        slipAngle = Mathf.Atan(vLateral.magnitude / vLongitudinal.magnitude) - steeringAngle * Mathf.Sign(vLongitudinal.z);
        if (float.IsNaN(slipAngle))
            return;

       // rigidbody.AddForceAtPosition(vLateral.normalized * slipAngle * cCorneringStiffnes, transform.position);
    }
}
