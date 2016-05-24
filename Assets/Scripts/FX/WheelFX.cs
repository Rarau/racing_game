using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectPool))]
public class WheelFX : MonoBehaviour 
{
    public WheelController wheel;

    public ParticleSystem particles;

    //public GameObject skidmarkPrefab;
    //private GameObject skidmarkObject;

    private AudioSource skidAudio;

    private int isSkidding;
    private Vector3[] lastPos = new Vector3[2];

    public Material rubberOnRoad;

    public float skidMarkWidth = 0.2f;
    Vector3 lastSkidmarkPos;
    public float heightOffset = 0.05f;
    public float sectionLenght = 0.125325f;

    public ObjectPool skidmarkPool;

    // Use this for initialization
    void Start() 
    {
        wheel = wheel == null ? GetComponent<WheelController>() : wheel;
        particles.playOnAwake = false;
        particles.enableEmission = false;

        skidmarkPool = GetComponent<ObjectPool>();
        if (skidmarkPool == null)
            skidmarkPool = gameObject.AddComponent<ObjectPool>();

        skidAudio = GetComponent<AudioSource>();
        if (skidAudio == null) 
        {
            Debug.LogError("No audio source, please add one skid audio to the tyres");
        }

        InitializePrefab();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

    void InitializePrefab()
    {
        GameObject skidmark = new GameObject("Skimdark");
        Mesh skidMarkMesh = new Mesh();

        skidmark.AddComponent<MeshFilter>();
        skidmark.AddComponent<MeshRenderer>();

        Vector3[] skidMarkMeshVertices = new Vector3[4];
        int[] skidMarkMeshTriangles;
        Vector2[] skidMarkMeshUVs = new Vector2[4];

        skidMarkMeshVertices[0] = new Vector3(skidMarkWidth, 0, sectionLenght);
        skidMarkMeshVertices[1] = new Vector3(-skidMarkWidth, 0, sectionLenght);
        skidMarkMeshVertices[2] = new Vector3(-skidMarkWidth, 0, -sectionLenght);
        skidMarkMeshVertices[3] = new Vector3(skidMarkWidth, 0, -sectionLenght);

        skidMarkMeshUVs[0] = new Vector2(1, 1);
        skidMarkMeshUVs[1] = new Vector2(0, 0);
        skidMarkMeshUVs[2] = new Vector2(0, 1);
        skidMarkMeshUVs[3] = new Vector2(1, 0);


        lastPos[0] = skidMarkMeshVertices[2];
        lastPos[1] = skidMarkMeshVertices[3];


        skidMarkMeshTriangles = new int[6] { 0, 2, 1, 2, 0, 3 };
        skidMarkMesh.vertices = skidMarkMeshVertices;
        skidMarkMesh.uv = skidMarkMeshUVs;
        skidMarkMesh.triangles = skidMarkMeshTriangles;

        skidmark.GetComponent<MeshFilter>().mesh = skidMarkMesh;
        skidmark.GetComponent<Renderer>().material = rubberOnRoad;
        skidmark.GetComponent<Renderer>().receiveShadows = false;

        skidmark.AddComponent<SelfDisable>().lifetime = 3.0f;

        skidmarkPool.objectPrefab = skidmark;
        skidmarkPool.initialSize = 90;
        //skidmarkPool.autoGrow = true;

        skidmarkPool.Initialize();
    }

    /// <summary>
    /// TO-DO: Optimize to use a single game object with a combined mesh
    /// or: pool the skidmark gameobjects
    /// </summary>
    void SkidMarkEffect()
    {
        //GameObject skidMark = new GameObject("Skid Mark");
        GameObject skidmark = skidmarkPool.Get();
        if(skidmark == null)
        {
            return;
        }
        skidmark.SetActive(true);

        if (Vector3.Distance(transform.position, lastSkidmarkPos) > sectionLenght)
        {
            {
                skidmark.transform.position = wheel.groundInfo.point + wheel.groundInfo.normal * heightOffset;
                skidmark.transform.rotation = Quaternion.LookRotation(wheel.rigidbody.velocity, wheel.groundInfo.normal);


                isSkidding = 1;

                lastSkidmarkPos = transform.position;
            }
        }


    }
}