using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class StraightLineTest : MonoBehaviour {

    public float engineForce = 10.0f;   // Constant force from the engine
    public float cDrag;                 // Drag (air friction constant)
    public float cRoll;                 // Rolling resistance (wheel friction constant)

    public float carMass = 1200.0f;     // Mass of the car in Kilograms

    Rigidbody rigidbody;

    bool accel;
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
    void Update()
    {
        accel = Input.GetKey(KeyCode.W);
    }

	// Update is called once per frame
	void FixedUpdate () {

        Vector3 velocity = rigidbody.velocity;
        Vector3 fTraction = transform.forward * (accel ? engineForce : 0.0f);
        Vector3 fDrag = -cDrag * velocity * velocity.magnitude;
        Vector3 fRoll = -cRoll * velocity;

        Vector3 fLong = fTraction + fDrag + fRoll;  // Total longitudinal force

        Vector3 acceleration = fLong / carMass;

        rigidbody.velocity += acceleration * Time.deltaTime;

	}
}
