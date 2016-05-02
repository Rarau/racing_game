using UnityEngine;
using System.Collections;

public class MenuViewObjectRotate : MonoBehaviour {
	
    public float rotateSpeed = 90.0f;//in degrees per second

	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotateSpeed);
	}
}
