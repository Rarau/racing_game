using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(WheelController))]
public class WheelFX : MonoBehaviour
{

    public WheelController wheel;

    public ParticleSystem particles;

    public GameObject skidmarkPrefab;
    private GameObject skidmarkObject;

    public AudioSource skidAudio;

    // Use this for initialization
    void Start()
    {
        wheel = wheel == null ? GetComponent<WheelController>() : wheel;
        particles.playOnAwake = false;
        particles.enableEmission = false;

        skidAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (skidAudio == null)
        {
            Debug.LogError("No audio source, please add one skid audio to the tyres");
            // enabled = false;
        }
        if (skidmarkPrefab == null)
        {
            Debug.LogError("No skidmark prefab, please attach it");
            enabled = false;
        }
        else
        {
            skidmarkPrefab.SetActive(false);
            skidmarkObject = GameObject.Instantiate(skidmarkPrefab);
            skidmarkObject.transform.parent = transform;
            skidmarkObject.transform.localPosition = -Vector3.up * 0.27f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Mathf.Abs(wheel.slipAngle) > 20.0f || Mathf.Abs(wheel.slipRatio) > 0.10f) && Mathf.Abs(wheel.rpm) > 2.10f)
        {
            //Debug.Log("ON " + name);
            ParticleSystem.EmissionModule em = particles.emission;
            particles.Play();
            em.enabled = true;
            if (wheel.IsGrounded)
            {
                skidmarkObject.SetActive(true);
            }
            if (!skidAudio.isPlaying) skidAudio.Play();
        }
        else
        {
            //Debug.Log("OFF " + name);
            ParticleSystem.EmissionModule em = particles.emission;
            particles.Stop();
            em.enabled = false;
            skidmarkObject.SetActive(false);

        }
    }
}
