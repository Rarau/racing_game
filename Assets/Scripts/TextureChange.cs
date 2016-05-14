using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

    public float meshHealth;

    CarController carController;

    Renderer renderer;

    float maxHealth = 100;

    // backward is not defined because is by defaults
    public bool forward = false;
    public bool left = false;
    public bool right = false;

    public float hitDistance = 2.4f;

    float healthDiscount = 0.5f;

    void Awake()
    {
        carController = transform.root.GetComponent<CarController>();
    }

    // Use this for initialization
    void Start () {
        meshHealth = maxHealth;
        renderer = this.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if (meshHealth >= maxHealth)
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

        if (forward)
            direction = this.transform.forward;
        else if (left)
            direction = -this.transform.right;
        else if (right)
            direction = this.transform.right;
        else // backward is by default
            direction = -this.transform.forward;

        if (Physics.Raycast(transform.position, direction, hitDistance))
        {
            meshHealth -= carController.currentSpeed * healthDiscount;
        }
    }
}
