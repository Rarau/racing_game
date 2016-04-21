using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WheelController))]
public class WheelFX : MonoBehaviour {

    WheelController wheel;

    public ParticleSystem particles;

    public GameObject skidmarkPrefab;
    public AudioSource skidAudio;
    float skidMarkWidth = 0.2f;
    private Vector3[] lastPos = new Vector3[2];

    public Material rubberOnRoad;

    int isSkidding;

    // Use this for initialization
    void Start () {
        wheel = GetComponent<WheelController>();

        skidAudio = (AudioSource)GetComponent(typeof(AudioSource));
        if (skidAudio == null)
        {
            Debug.LogError("No audio source, please add one skid audio to the tyres");
            enabled = false;
        }
        if (skidmarkPrefab == null)
        {
            Debug.LogError("No skidmark prefab, please attach it");
            enabled = false;
        }
        skidmarkPrefab.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if ((Mathf.Abs(wheel.slipAngle) > 30.0f || Mathf.Abs(wheel.slipRatio) > 1.0f) && Mathf.Abs(wheel.rpm) > 10.0f)
        {
            particles.enableEmission = true;
            if (wheel.isGrounded)  
            {
                //skidmarkPrefab.SetActive(true);
                SkidMarkEffect();
            }
            if (!skidAudio.isPlaying) skidAudio.Play();
        }
        else
        {
            particles.enableEmission = false;
            skidmarkPrefab.SetActive(false);
        } 
    }

    void SkidMarkEffect()
    {
        GameObject skidMark = new GameObject("Skid Mark");
        Mesh skidMarkMesh = new Mesh();
        Vector3[] skidMarkMeshVertices = new Vector3[4];
        int[] skidMarkMeshTriangles;

        skidMark.AddComponent<MeshFilter>();
        skidMark.AddComponent<MeshRenderer>();
        skidMark.name = "Skid Mark Mesh";

        if (isSkidding == 0)
        {
            skidMarkMeshVertices[0] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, 0.01f, 0);
            skidMarkMeshVertices[1] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, 0.01f, 0);
            skidMarkMeshVertices[2] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, 0.01f, 0);
            skidMarkMeshVertices[3] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, 0.01f, 0);

            lastPos[0] = skidMarkMeshVertices[2];
            lastPos[1] = skidMarkMeshVertices[3];

            isSkidding = 1;
        }
        else
        {
            skidMarkMeshVertices[0] = lastPos[1];
            skidMarkMeshVertices[1] = lastPos[0];

            skidMarkMeshVertices[2] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(-skidMarkWidth, 0.01f, 0);
            skidMarkMeshVertices[3] = wheel.prevPos + Quaternion.Euler(transform.eulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z) * new Vector3(skidMarkWidth, 0.01f, 0);

            lastPos[0] = skidMarkMeshVertices[2];
            lastPos[1] = skidMarkMeshVertices[3];

            //isSkidding = 0;
        }

        skidMarkMeshTriangles = new int[6] { 0, 1, 2, 2, 3, 0 };
        skidMarkMesh.vertices = skidMarkMeshVertices;
        skidMarkMesh.triangles = skidMarkMeshTriangles;

        skidMark.GetComponent<MeshFilter>().mesh = skidMarkMesh;
        skidMark.GetComponent<Renderer>().material = rubberOnRoad;
        skidMark.AddComponent<SelfDestroy>();
    }
}
