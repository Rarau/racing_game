using UnityEngine;
using System.Collections;

public class EngineSound : MonoBehaviour
{

    public AudioSource engineAudio;
    public CarController car;

    void Start()
    {
        car = GetComponent<CarController>();
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
            float ratioRPM = car.myCurrentRPM / car.rpmMax * 50.0f;

            //float pitch = 1.0f + 2.0f * car.rpm / car.rpmMax;//Mathf.Clamp(1.0f + ((car.kilometerPerHour - 10) / (car.maxSpeed)), 1.0f, 8.0f);
            //float pitch = Mathf.Clamp(1.0f + ((car.kilometerPerHour - 10) / (car.maxSpeed)), 1.0f, 8.0f);
            float pitch = Mathf.Clamp(0.35f * ratioRPM, 0.0f, 8.0f);
            engineAudio.pitch = pitch;
            //engineAudio.volume += 0.1f;
        }
    }

}
 