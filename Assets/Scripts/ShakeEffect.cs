using UnityEngine;
using System.Collections;

public class ShakeEffect : MonoBehaviour {
    public float duration = 0.85f;
    public float magnitude = 0.5f;
    public float frequency = 5.0f;

    public bool loop;
    public bool playOnAwake;
    public bool selfDisable = true;
	// Use this for initialization
	void OnEnable () {
        if (playOnAwake)
            StartCoroutine(Shake());
	}
	

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPos = transform.localPosition;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Mathf.PerlinNoise(Time.time * frequency, 0.0f) * 2.0f - 1.0f; //Random.value * 2.0f - 1.0f;
            float y = Mathf.PerlinNoise(0.0f, Time.time * frequency) * 2.0f - 1.0f;//Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;


            transform.localPosition = new Vector3(x + originalCamPos.x, y + originalCamPos.y, originalCamPos.z);

            yield return null;
        }

        transform.localPosition = originalCamPos;
       enabled = !selfDisable;
       if (loop)
           yield return StartCoroutine(Shake());
    }

}
