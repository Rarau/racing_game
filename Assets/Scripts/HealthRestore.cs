using UnityEngine;
using System.Collections;

public class HealthRestore : MonoBehaviour {

    public float lifetime = 20.0f;
    private float startTime;

    public Component[] carPieces;

    GameObject spannerKey;

    // Use this for initialization
    void Start () {
        //gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        spannerKey = GameObject.Find("SpannerKey");
        spannerKey.SetActive(true);
        this.GetComponent<Renderer>().enabled = false;
        startTime = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        if (!spannerKey.activeSelf &&
             (startTime + lifetime) < Time.timeSinceLevelLoad)
        {
            //gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            spannerKey.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled
        if (spannerKey.activeSelf && 
            collider.gameObject.tag == "WrenchTag")
        {
            //gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            spannerKey.SetActive(false);
            startTime = Time.timeSinceLevelLoad;

            carPieces = collider.gameObject.transform.root.GetComponentsInChildren<MeshChange>();
            foreach (MeshChange pieces in carPieces)
            {
                pieces.meshHealth = 101;
            }
        }
    }
}
