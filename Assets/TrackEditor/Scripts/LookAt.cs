using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
    public Transform h1, h2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);

        foreach (Transform t in transform)
            Debug.DrawLine(transform.position, t.position, Color.white);
	}
}
