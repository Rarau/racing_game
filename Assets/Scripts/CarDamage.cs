using UnityEngine;
using System.Collections;

public class CarDamage : MonoBehaviour
{

    //private CarController carController;

    public Mesh[] swapMesh;

    public int carHealth;

    private MeshFilter meshFilter;

    // Use this for initialization
    void Start()
    {
        //carController = transform.root.GetComponent<CarController>();

        carHealth = 100;

        meshFilter = this.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Car Health" + carHealth);

        if (carHealth < 100)
        {
            meshFilter.mesh = swapMesh[1];
        }
        else
        {
            meshFilter.mesh = swapMesh[0];
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enviroment")
        {
            Debug.Log("something hit");
            carHealth -= 5;
        }
        Debug.Log("collision");
        carHealth -= 10;
    }
}
