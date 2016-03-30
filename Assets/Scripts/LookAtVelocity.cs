using UnityEngine;
using System.Collections;

public class LookAtVelocity : MonoBehaviour
{
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
	}
}
