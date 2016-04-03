using UnityEngine;
using System.Collections;

public class SelfDisable : MonoBehaviour
{
    public float lifetime = 1.0f;
    private float startTime;

    void OnEnable () 
    {
        startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ((startTime + lifetime) < Time.timeSinceLevelLoad)
            gameObject.SetActive(false);
	}

    void Start()
    {
        gameObject.SetActive(false);
    }
}
