using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuViewSpinner : MenuView {

    public Camera viewCamera;

    public Vector3 center = new Vector3(0,0,0);
    public float radius = 10.0f;
    public float optionSpinSpeed = 90.0f;//in degrees per second
    public float timeToTurn = 1.0f;//in seconds
    // ^ MAKE SURE THIS IS <= CONTROLLER INPUT PAUSE ; OTHERWISE CAUSES UNDEFINED BEHAVIOUR
    private List<MenuViewOptionSpinner> options;

    private GameObject spinner;
    private float curAngle = 0.0f;
    private float targetAngle = 0.0f;
    private bool canMove = true;

    // Use this for initialization
    void Start () {
        menu = GetComponentInParent<Menu>();

        options = new List<MenuViewOptionSpinner>();
        GetComponentsInChildren<MenuViewOptionSpinner>(options);
        //set up meshes / spinner
        spinner = new GameObject("MenuViewSpinner");
        spinner.transform.SetParent(transform);
        spinner.transform.position = center;
        //make spinner face camera
        spinner.transform.LookAt(viewCamera.transform);
        //spinner.transform.up = Vector3.up;
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
	}
	
	// Update is called once per frame
	void Update () {
	}

    public override void GoToOption(int option)
    {
        if (canMove)
        {
            canMove = false;
            targetAngle = (((360.0f / options.Count) * option)) % 360;
            StartCoroutine("RotateToTargetAngle");
        }
    }

    IEnumerator RotateToTargetAngle()
    {
        float upDistance;
        float downDistance;
        float curTurn = 0.0f;
        float nextAngle = 0.0f;
        float curTime = 0.0f;
        bool goUp;
        //cases: must go over 360 (curAngle = 270; targetAngle = 45) ; use modulus
        //must go under 0 (curAngle = 45; targetAngle = 270) ; use 360 - x
        //inside normal bounds ; just calculate distance
        //distance moved should always be 180 or less

        //calculate distance from target
        if(curAngle <= targetAngle)
        {
            upDistance = targetAngle - curAngle;
            downDistance = 360.0f - upDistance;
        }
        else
        {
            downDistance = curAngle - targetAngle;
            upDistance = 360.0f - downDistance;
        }
        if (downDistance > upDistance) goUp = true;
        else goUp = false;
        //print("curAngle: " + curAngle + "\ntargetAngle: " + targetAngle + "\nupDistance: " + upDistance + "\ndownDistance: " + downDistance);

        while (curAngle != targetAngle)
        {
            if (goUp)
            {
                curTurn = upDistance * (Time.deltaTime / timeToTurn);
                nextAngle = (curAngle + curTurn);
                //if (nextAngle > 360.0f && targetAngle != 0.0f) nextAngle -= 360.0f;
                if (nextAngle > targetAngle)
                {
                   if(curAngle + upDistance >= 360.0f)//going through modulus
                   {
                        if((curAngle + curTurn) >= 360.0f && (curAngle + curTurn) % 360 >= targetAngle)
                        { 
                            curTurn = (360.0f - curAngle) + targetAngle;
                        }
                   }
                   else
                   {
                       curTurn = targetAngle - curAngle;
                   }
                }
            }
            else
            {
                curTurn = -downDistance * (Time.deltaTime / timeToTurn);
                nextAngle = curAngle + curTurn;
                if (nextAngle < 0.0f && targetAngle != 0.0f) nextAngle += 360.0f;
                if(nextAngle < targetAngle)
                {
                    curTurn = targetAngle - curAngle;
                }
            }
            curAngle += curTurn;
            if (curAngle >= 360.0f) curAngle -= 360.0f;
            else if (curAngle < 0.0f) curAngle += 360.0f;
            curTime += Time.deltaTime;
            spinner.transform.RotateAround(spinner.transform.position, spinner.transform.up, curTurn);//final position
            yield return null;
        }
        canMove = true;
    }

    public override void AddOption(GameObject newOption)
    {
        if (options == null) options = new List<MenuViewOptionSpinner>();
        newOption.transform.SetParent(transform, false);
        options.Add(newOption.GetComponent<MenuViewOptionSpinner>());
    }
}
