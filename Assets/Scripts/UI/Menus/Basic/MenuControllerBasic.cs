using UnityEngine;
using System.Collections;

public class MenuControllerBasic : MenuController {

    private bool acceptAxis;
    public float timeOut = 0.3f;//time before another input is accepted, in seconds

    void Start()
    {
        menu = GetComponentInParent<Menu>();
        acceptAxis = true;
    }

	// Update is called once per frame
	void Update () {
        if (acceptAxis)
        {
            if (Input.GetAxis("P1_Horizontal") > 0)//right
            {
                menu.NextOption();
                acceptAxis = false;
                StartCoroutine("AxisCountdown");
            }
            else if (Input.GetAxis("P1_Horizontal") < 0)//left
            {
                menu.PrevOption();
                acceptAxis = false;
                StartCoroutine("AxisCountdown");
            }
        }
        if (Input.GetButtonDown("Submit"))
        {
            menu.SelectCurOption();
        }
	}

    IEnumerator AxisCountdown()
    {
        yield return new WaitForSeconds(timeOut);
        acceptAxis = true;
    }
    
}
