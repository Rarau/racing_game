using UnityEngine;
using System.Collections;

public class EngineSound : MonoBehaviour
{

    public AudioSource engineAudio;
    public CarController car;
    public AudioClip[] clips;

    void Start()
    {
        car = transform.root.GetComponent<CarController>();
        engineAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (engineAudio == null)
        {
            Debug.Log("No audio source, please add one engine noise to the car");
        }
        car.gearShiftEvent += OnGearShift;
    }

    void OnGearShift(int numGear)
    {
        if (numGear > 0 && numGear < clips.Length)
        {
            engineAudio.clip = clips[numGear];
            engineAudio.Play();
        }
        else
        {
            Debug.LogError("No audio clip for gear " + numGear);
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

            engineAudio.pitch = car.virtualRPM * 2f + 0.5f;
            engineAudio.volume = 0.35f*car.currentGear;
        }
    }

}
 