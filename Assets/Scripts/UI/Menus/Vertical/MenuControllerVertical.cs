using UnityEngine;
using System.Collections;

public class MenuControllerVertical : MenuController {

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
            if (Input.GetAxis("P1_Vertical") > 0)//up
            {
                menu.PrevOption();
                acceptAxis = false;
                StartCoroutine("AxisCountdown");
            }
            else if (Input.GetAxis("P1_Vertical") < 0)//down
            {
                menu.NextOption();
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
