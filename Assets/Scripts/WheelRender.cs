using UnityEngine;
using System.Collections;

public class WheelRender : MonoBehaviour {
    public WheelTest wheel;
    Vector3 localPos;
    Renderer renderer;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        localPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //transform.localPosition += renderer.bounds.center;
        transform.localPosition = Vector3.zero;
        transform.RotateAround(renderer.bounds.center, transform.up, wheel.angularVelocity * Time.deltaTime);
        transform.localPosition = localPos - wheel.transform.up * wheel.wheelPosY;

        //transform.localPosition -= renderer.bounds.center;
	}
}
