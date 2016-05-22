using UnityEngine;
using System.Collections;

public class LookAtVelocity : MonoBehaviour
{
    public Vector3 rotOffset;

    Rigidbody rigidbody;
    public float smoothness  = 3.0f;
	// Use this for initialization
	void Start () 
    {
        rigidbody = transform.root.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        transform.forward = Vector3.Lerp(transform.forward, rigidbody.velocity, Time.deltaTime * smoothness);

        if (rotOffset.magnitude > 0.01f)
            transform.forward = Vector3.Lerp(transform.forward, rigidbody.transform.TransformDirection((rotOffset)).normalized * rigidbody.velocity.magnitude, Time.deltaTime * smoothness);
        
	}
}
