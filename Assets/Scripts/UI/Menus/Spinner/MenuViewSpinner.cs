using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuViewSpinner : MenuView {

    public Vector3 center = new Vector3(0,0,0);
    public float radius = 10.0f;
    public float optionSpinSpeed = 1.0f;//in degrees per second
    public float rotateSpeed = 10.0f;//in degrees per second
    public List<MenuViewOptionSpinner> options;

    private float curAngle;//in radians

    // Use this for initialization
    void Start () {
        menu = GetComponentInParent<Menu>();

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override void GoToOption(int option)
    {

    }

    public override void AddOption(GameObject newOption)
    {

    }
}
