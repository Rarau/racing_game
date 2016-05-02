using UnityEngine;
using System.Collections;

public class MeshChange : MonoBehaviour {

    public float meshHealth;

    CarController carController;

    Rigidbody rb;

    public bool looseParts;

    public bool alreadyDetached;

    public bool forward = true;

    void Awake()
    {
        carController = transform.root.GetComponent<CarController>();
    }

    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        meshHealth = 100;
        looseParts = true;
        alreadyDetached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (meshHealth >= 100)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        }
        else if (meshHealth > 0 && meshHealth < 80)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
        }
        else if (meshHealth <= 0 && looseParts && !alreadyDetached)
        {
            addRigidBody(this.gameObject);
            alreadyDetached = true;
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.tag == "Enviroment")
    //    {
    //        Debug.Log("something hit");
    //        meshHealth -= carController.currentSpeed * .1f;
    //    }
        

    //    Debug.Log("collision");
    //    meshHealth -= 10;
    //}

    void FixedUpdate()
    {
        Vector3 direction;
        if (forward)
            direction = transform.TransformDirection(Vector3.forward);
        else
            direction = transform.TransformDirection(Vector3.back);

        Debug.DrawLine(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, 2.4f))
        {
            // Do something if hit
            meshHealth -= carController.currentSpeed * .1f;
        }
    }

    private void addRigidBody(GameObject gameObject)
    {
        Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
        //rigidBody.mass = 1;
        //rigidBody.useGravity = true;
        //foreach (Transform t in gameObject.transform)
        //{
        //    addRigidBody(t.gameObject);
        //}
    }
}
