using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WheelController))]
public class WheelFX : MonoBehaviour {

    WheelController wheel;

    public ParticleSystem particles;

	// Use this for initialization
	void Start () {
        wheel = GetComponent<WheelController>();
	}
	
	// Update is called once per frame
	void Update () {
        if ((Mathf.Abs(wheel.slipAngle) > 30.0f || Mathf.Abs(wheel.slipRatio) > 1.0f ) && Mathf.Abs(wheel.rpm) > 2.0f)
        {
            particles.enableEmission = true;
        }
        else particles.enableEmission = false;
	}
}
