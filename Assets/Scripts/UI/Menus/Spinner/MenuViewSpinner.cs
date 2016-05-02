using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuViewSpinner : MenuView {

    public Camera viewCamera;

    public Vector3 center = new Vector3(0,0,0);
    public float radius = 10.0f;
    public float optionSpinSpeed = 90.0f;//in degrees per second
    public float rotateSpeed = 10.0f;//in degrees per second
    public float curAngle = 0.0f;
    public List<MenuViewOptionSpinner> options;

    //PLACEHOLDER
    public Mesh testMesh;
    public Material testMaterial;
    //PLACEHOLDER ENDS

    private GameObject spinner;

    // Use this for initialization
    void Start () {
        menu = GetComponentInParent<Menu>();
        //set up meshes / spinner
        spinner = new GameObject("MenuViewSpinner");
        spinner.transform.position = center;

        //PLACEHOLDER CODE (gives spinner cube graphic for testing)
        MeshFilter filter = spinner.AddComponent<MeshFilter>();
        filter.mesh = testMesh;
        MeshRenderer renderer = spinner.AddComponent<MeshRenderer>();
        renderer.material = testMaterial;
        //END PLACEHOLDER
        //make spinner face camera
        spinner.transform.LookAt(viewCamera.transform);
        spinner.transform.up = Vector3.up;
        if(options.Count > 0)
        {
            GameObject curObject;
            MeshFilter curFilter;
            MeshRenderer curRenderer;
            MenuViewObjectRotate curRotater;

            for(int i = 0; i < options.Count; i++)
            {
                curObject = new GameObject("Spinner Option "+i);
                curObject.transform.SetParent(spinner.transform);
                curObject.transform.localPosition = Vector3.zero;
                curObject.transform.Translate(radius * (float)Math.Sin(((Math.PI * 2) / options.Count) * i), 0, radius * (float)Math.Cos(((Math.PI * 2) / options.Count) * i), spinner.transform);
                curObject.transform.localEulerAngles = Vector3.zero;
                curObject.transform.RotateAround(curObject.transform.position, curObject.transform.up, (float)(360 / options.Count) * i);
                curRotater = curObject.AddComponent<MenuViewObjectRotate>();
                curRotater.rotateSpeed = optionSpinSpeed;
                print(curRotater.rotateSpeed);
                //set position
                curFilter = curObject.AddComponent<MeshFilter>();
                curFilter.mesh = options[i].mesh;
                curRenderer = curObject.AddComponent<MeshRenderer>();
                curRenderer.material = options[i].material;
            }
        }
        //spinner.transform.RotateAround(spinner.transform.position, spinner.transform.up, );
	}
	
	// Update is called once per frame
	void Update () {
        //spinner.transform.RotateAround(spinner.transform.position, spinner.transform.up, 1);
	}

    public override void GoToOption(int option)
    {
        spinner.transform.LookAt(viewCamera.transform);
        spinner.transform.up = Vector3.up;
        spinner.transform.RotateAround(spinner.transform.position, spinner.transform.up, ((360.0f / options.Count) * option) + 180);
        /*Transform[] spinChildren = spinner.transform.GetComponentsInChildren<Transform>();
        for(int i = 0; i < spinner.transform.childCount; i++)
        {
            spinChildren[i].up = Vector3.up;
            //spinChildren[i].localPosition = Vector3.zero;
            //spinChildren[i].Translate(radius * (float)Math.Sin(((Math.PI * 2) / options.Count) * i), 0, radius * (float)Math.Cos(((Math.PI * 2) / options.Count) * i), spinner.transform);
        }*/
    }

    public override void AddOption(GameObject newOption)
    {

    }
}
