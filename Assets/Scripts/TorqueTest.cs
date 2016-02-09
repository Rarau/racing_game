using UnityEngine;
using System.Collections;

public class TorqueTest : MonoBehaviour {
    public Rigidbody rb;
    public WheelController ct;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddTorque(Vector3.up * 10.0f);
            rb.maxAngularVelocity = float.MaxValue;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("RB w: " + rb.angularVelocity.y);
        GUILayout.Label("CT w: " + ct.AngularVelocity);
    }
}
