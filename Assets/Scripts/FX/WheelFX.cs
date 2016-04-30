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
    private float skidMarkWidth = 0.2f;
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

    void SkidMarkEffect() {
        //GameObject skidMark = new GameObject("Skid Mark");
        Mesh skidMarkMesh = new Mesh();
        Vector3[] skidMarkMeshVertices = new Vector3[4];
        int[] skidMarkMeshTriangles;
        float wheelHeight = -0.05f;

        int vertexOffset = skidMarkMesh.vertexCount;

        if (isSkidding == 0) {
            skidMarkMeshVertices[0 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, wheelHeight, 0);
            skidMarkMeshVertices[1 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, wheelHeight, 0);
            skidMarkMeshVertices[2 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, wheelHeight, 0);
            skidMarkMeshVertices[3 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, wheelHeight, 0);

            lastPos[0] = skidMarkMeshVertices[2 + vertexOffset];
            lastPos[1] = skidMarkMeshVertices[3 + vertexOffset];

            isSkidding = 1;
        } else {
            skidMarkMeshVertices[0 + vertexOffset] = lastPos[1];
            skidMarkMeshVertices[1 + vertexOffset] = lastPos[0];

            skidMarkMeshVertices[2 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, wheelHeight, 0);
            skidMarkMeshVertices[3 + vertexOffset] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, wheelHeight, 0);

            lastPos[0] = skidMarkMeshVertices[2 + vertexOffset];
            lastPos[1] = skidMarkMeshVertices[3 + vertexOffset];
        }

        

        skidMarkMeshTriangles = new int[6] { 0, 1, 2, 2, 3, 0 };
        skidMarkMesh.vertices = skidMarkMeshVertices;
        skidMarkMesh.triangles = skidMarkMeshTriangles;

        //skidMark.GetComponent<MeshFilter>().mesh = skidMarkMesh;
        //skidMark.GetComponent<Renderer>().material = rubberOnRoad;
        //skidMark.AddComponent<SelfDestroy>();
    }
}