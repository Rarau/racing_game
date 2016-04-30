using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(WheelController))]
public class WheelFX : MonoBehaviour {
    public WheelController wheel;

    public ParticleSystem particles;

    //public GameObject skidmarkPrefab;
    //private GameObject skidmarkObject;

    private AudioSource skidAudio;

    private int isSkidding;
    private Vector3[] lastPos = new Vector3[2];

    public Material rubberOnRoad;

    // Use this for initialization
    void Start() {
        wheel = wheel == null ? GetComponent<WheelController>() : wheel;
        particles.playOnAwake = false;
        particles.enableEmission = false;

        skidAudio = GetComponent<AudioSource>();
        if (skidAudio == null) {
            Debug.LogError("No audio source, please add one skid audio to the tyres");
            // enabled = false;
        }
        //if (skidmarkPrefab == null)
        //{
        //    Debug.LogError("No skidmark prefab, please attach it");
        //    enabled = false;
        //}
        //else
        //{
        //    skidmarkPrefab.SetActive(false);
        //    skidmarkObject = GameObject.Instantiate(skidmarkPrefab);
        //    skidmarkObject.transform.parent = transform;
        //    skidmarkObject.transform.localPosition = -Vector3.up * 0.27f;
        //}
    }

    // Update is called once per frame
    void Update() {
        if ((Mathf.Abs(wheel.slipAngle) > 20.0f || Mathf.Abs(wheel.slipRatio) > 0.10f) && Mathf.Abs(wheel.rpm) > 2.10f) {
            //Debug.Log("ON " + name);
            ParticleSystem.EmissionModule em = particles.emission;
            particles.Play();
            em.enabled = true;
            if (!skidAudio.isPlaying) skidAudio.Play();
            SkidMarkEffect();
        } else {
            //Debug.Log("OFF " + name);
            ParticleSystem.EmissionModule em = particles.emission;
            particles.Stop();
            em.enabled = false;
            isSkidding = 0;

        }
        //Debug.Log("isgrounded: " + wheel.IsGrounded);
    }

    private float skidMarkWidth = 0.2f;
    Vector3 lastSkidmarkPos;
    void SkidMarkEffect()
    {
        GameObject skidMark = new GameObject("Skid Mark");
        Mesh skidMarkMesh = new Mesh();
        Vector3[] skidMarkMeshVertices = new Vector3[4];
        int[] skidMarkMeshTriangles;
        float wheelHeight = 0.25325f;

        skidMark.AddComponent<MeshFilter>();
        skidMark.AddComponent<MeshRenderer>();
        skidMark.name = "Skid Mark Mesh";

        if (Vector3.Distance(transform.position, lastSkidmarkPos) > wheelHeight)
        {
            if (isSkidding == 0)
            {
                skidMark.transform.position = wheel.groundInfo.point + wheel.groundInfo.normal * 0.12f;
                skidMark.transform.rotation = Quaternion.LookRotation(wheel.rigidbody.velocity, wheel.groundInfo.normal);

                skidMarkMeshVertices[0] = new Vector3(skidMarkWidth, 0, wheelHeight);
                skidMarkMeshVertices[1] = new Vector3(-skidMarkWidth, 0, wheelHeight);
                skidMarkMeshVertices[2] = new Vector3(-skidMarkWidth, 0, -wheelHeight);
                skidMarkMeshVertices[3] = new Vector3(skidMarkWidth, 0, -wheelHeight);

                lastPos[0] = skidMarkMeshVertices[2];
                lastPos[1] = skidMarkMeshVertices[3];

                isSkidding = 1;

                lastSkidmarkPos = transform.position;

            }
            else
            {
                skidMark.transform.position = wheel.groundInfo.point + wheel.groundInfo.normal * 0.12f; ;
                skidMark.transform.rotation = Quaternion.LookRotation(wheel.rigidbody.velocity, wheel.groundInfo.normal);

                skidMarkMeshVertices[0] = new Vector3(skidMarkWidth, 0, wheelHeight);
                skidMarkMeshVertices[1] = new Vector3(-skidMarkWidth, 0, wheelHeight);
                skidMarkMeshVertices[2] = new Vector3(-skidMarkWidth, 0, -wheelHeight);
                skidMarkMeshVertices[3] = new Vector3(skidMarkWidth, 0, -wheelHeight);

                lastPos[0] = skidMarkMeshVertices[2];
                lastPos[1] = skidMarkMeshVertices[3];

                lastSkidmarkPos = transform.position;

            }
        }
        skidMarkMeshTriangles = new int[6] { 0, 2, 1, 2, 0, 3 };
        skidMarkMesh.vertices = skidMarkMeshVertices;
        skidMarkMesh.triangles = skidMarkMeshTriangles;

        skidMark.GetComponent<MeshFilter>().mesh = skidMarkMesh;
        skidMark.GetComponent<Renderer>().material = rubberOnRoad;
        skidMark.AddComponent<SelfDestroy>();

    }
}