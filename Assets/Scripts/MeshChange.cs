using UnityEngine;
using System.Collections;
using System;

public class MeshChange : MonoBehaviour {

    public float meshHealth;

    CarController carController;

    ChildInfo childInfo;

    Rigidbody rb;

    public bool looseParts = true;

    public bool alreadyDetached = false;

    // backward is not defined because is by defaults
    public bool forward = false;
    public bool left = false;
    public bool right = false;

    public float hitDistance = 2.4f;

    float healthDiscount = 0.1f;

    public bool usesPhysics = true;

    public GameObject damageParticles;
    GameObject cloneDamageParticles;

    public bool isEngine;

    bool isSmoke = false;

    void Awake()
    {
        carController = transform.root.GetComponent<CarController>();
        childInfo = transform.GetChild(0).GetComponent<ChildInfo>();
    }

    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        if (usesPhysics)
            gameObject.transform.GetChild(1).GetComponent<Collider>().enabled = false;
        meshHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //MeshRenderer[] childrenRenderes = GetComponentsInChildren<MeshRenderer>();
        //foreach(MeshRenderer renderer in childrenRenderes)

        //{
        //    renderer.material.mainTexture = 
        //    }

        if (meshHealth >= 100)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            if (alreadyDetached && usesPhysics)
            {
                Destroy(gameObject.transform.GetChild(1).GetComponent<Rigidbody>());
                gameObject.transform.GetChild(1).rotation = gameObject.transform.GetChild(0).rotation;
                gameObject.transform.GetChild(1).position = gameObject.transform.GetChild(0).position;
                gameObject.transform.GetChild(1).GetComponent<Collider>().enabled = false;
                alreadyDetached = false;
            }
        }
        else if (meshHealth > 0 && meshHealth < 80)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
        }
        else if (meshHealth <= 0 && looseParts && !alreadyDetached && usesPhysics)
        {
            gameObject.transform.GetChild(1).GetComponent<Collider>().enabled = true;
            addRigidBody(this.transform.GetChild(1).gameObject);
            alreadyDetached = true;
        }

        if (isEngine)
        {
            if (alreadyDetached && !isSmoke)
            {
                cloneDamageParticles = (GameObject)Instantiate(damageParticles, transform.position, transform.rotation);
                cloneDamageParticles.transform.rotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
                cloneDamageParticles.transform.parent = transform;
                isSmoke = true;
            }
            else if (!alreadyDetached && isSmoke)
            {
                Destroy(cloneDamageParticles, 1.0f);
                isSmoke = false;
            }
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
            direction = this.transform.forward;
        else if(left)
            direction = -this.transform.right;
        else if (right)
            direction = this.transform.right;
        else // backward is by default
            direction = -this.transform.forward;

        Debug.DrawRay(childInfo.GetPosition(), direction * hitDistance/2, Color.magenta);

        // Do something if hit
        if (Physics.Raycast(childInfo.GetPosition(), direction, hitDistance))
        {
            //Debug.Log("position : " + childInfo.GetPosition() + " "+ childInfo.name);
            meshHealth -= Mathf.Clamp(carController.currentSpeed * healthDiscount, 0, 150);
            
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
