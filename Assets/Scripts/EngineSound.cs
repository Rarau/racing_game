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
            float pitch = Mathf.Clamp(1.0f + ((car.kilometerPerHour - 10) / (car.maxSpeed)), 1.0f, 8.0f);
            engineAudio.pitch = pitch;
            //engineAudio.volume += 0.1f;
        }
    }

}
