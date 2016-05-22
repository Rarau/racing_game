using UnityEngine;
using System.Collections;

public class HealthRestore : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.GetChild(0).rotation = true;
    }
}
