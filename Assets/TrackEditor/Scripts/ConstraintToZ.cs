using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class ConstraintToZ : MonoBehaviour {
    private Vector3 startPos;
	// Use this for initialization
	void Start () {
        startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 p = transform.localPosition;
        p.x = 0.0f;
        p.y = 0.0f;
        transform.localPosition = p;
	}
}
