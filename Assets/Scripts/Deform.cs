using UnityEngine;
using System.Collections;

public class Deform : MonoBehaviour
{
    private Material material;
    public CarController car;
    public AudioSource crashAudio;

    private float destructionLevel = 0.0f;

    void Start()
    {
        car = transform.root.GetComponent<CarController>();
        material = GetComponent<Renderer>().material;

        crashAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (crashAudio == null)
        {
            Debug.Log("No audio source, please add one engine noise to the car");
        }
    }

    void Update()
    {
        if (car.isAccident && destructionLevel<1.0f)
        {
            destructionLevel = 1.0f;
            material.SetFloat("_Displacement", destructionLevel);
            crashAudio.Play();
        }


    }
}
