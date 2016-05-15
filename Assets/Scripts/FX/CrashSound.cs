using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CrashSound : MonoBehaviour {

    public AudioClip impact;
    AudioSource crashAudio;

    public float lifetime = 6.0f;
    private float startTime;

    void OnEnable()
    {
        startTime = Time.timeSinceLevelLoad;
    }

    // Use this for initialization
    void Start () {
        crashAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!crashAudio.isPlaying && gameObject.active && (startTime + lifetime) < Time.timeSinceLevelLoad)
        {
            crashAudio.PlayOneShot(impact, 0.7f);

        }
    }
}
