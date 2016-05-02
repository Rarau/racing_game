using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

    public float meshHealth;

    CarController carController;

    Renderer renderer;

    void Awake()
    {
        carController = transform.root.GetComponent<CarController>();
    }

    // Use this for initialization
    void Start () {
        meshHealth = 100;
        renderer = this.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if (meshHealth >= 100)
        {
            renderer.material.mainTexture = Resources.Load("Bugatti_Alb") as Texture;
            //this.SetTexture("Texture", Resources.Load("Bugatti_Alb") as Texture); // how to load normal maps??
        }
        else
        {
            renderer.material.mainTexture = Resources.Load("Bugatti_D1_Alb") as Texture;
        }
    }

    void FixedUpdate()
    {
        Vector3 direction;
        direction = transform.TransformDirection(Vector3.forward);

        Debug.DrawLine(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, 2.4f))
        {
            meshHealth -= carController.currentSpeed * .5f;
        }
    }
}
