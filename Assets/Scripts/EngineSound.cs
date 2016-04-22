using UnityEngine;
using System.Collections;

public class EngineSound : MonoBehaviour
{

    public AudioSource engineAudio;
    public CarController car;

    void Start()
    {
        car = transform.root.GetComponent<CarController>();
        engineAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (engineAudio == null)
        {
            Debug.Log("No audio source, please add one engine noise to the car");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (engineAudio != null)
        {
            float ratioRPM = (car.virtualRPM / car.rpmMax * 2) +
                            (car.currentSpeed / car.maxSpeed) +
                            (car.currentGear / car.maxGears);

            //Debug.Log(ratioRPM);

            float pitch = Mathf.Clamp(ratioRPM, 1.0f, 3.0f);

            engineAudio.pitch = pitch;
            engineAudio.volume = 0.25f*car.currentGear;
        }
    }

}
 