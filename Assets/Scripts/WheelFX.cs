using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WheelController))]
public class WheelFX : MonoBehaviour {

    WheelController wheel;

    public ParticleSystem particles;

    public GameObject skidmarkPrefab;
    public AudioSource skidAudio;

    // Use this for initialization
    void Start () {
        wheel = GetComponent<WheelController>();
        skidmarkPrefab.SetActive(false);

        skidAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (skidAudio == null)
        {
            Debug.Log("No audio source, please add one skid audio to the tyres");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if ((Mathf.Abs(wheel.slipAngle) > 30.0f || Mathf.Abs(wheel.slipRatio) > 1.0f) && Mathf.Abs(wheel.rpm) > 2.0f)
        {
            particles.enableEmission = true;
            skidmarkPrefab.SetActive(true);
            skidAudio.Play();
        }
        else
        {
            particles.enableEmission = false;
            skidmarkPrefab.SetActive(false);
        } 
    }
}
