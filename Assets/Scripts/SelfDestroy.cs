using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {
    public float lifetime = 3.0f;
    private float startTime;

    void OnEnable() {
        startTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update() {
        if ((startTime + lifetime) < Time.timeSinceLevelLoad)
            Destroy(gameObject);
    }
}