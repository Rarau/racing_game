using UnityEngine;
using System.Collections;

public class WheelChange : MonoBehaviour {

    //public CarController carController;
    public WheelController wheelController;

    void Awake()
    {
        //carController = transform.root.GetComponent<CarController>();
        //wheelController = transform.root.GetComponent<wheelController>();
    }

    void Start ()
    {
        gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }
    // Update is called once per frame
    void Update () {
        //Debug.Log(wheelController.rpm);
        if (wheelController.rpm < 200)
        { // slow
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }
        else if (wheelController.rpm > 400)
        { // fast
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        }
        else
        { // middle
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }
    }
}
