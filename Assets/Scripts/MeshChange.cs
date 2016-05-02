using UnityEngine;
using System.Collections;

public class MeshChange : MonoBehaviour {

    public float meshHealth;

    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        meshHealth = 100;
    }
    // Update is called once per frame
    void Update()
    {
        if (meshHealth < 100)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;

        }
        else 
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        }
    }
}
