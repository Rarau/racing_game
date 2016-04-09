using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(BezierCurve), typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class SplineExtrude : MonoBehaviour
{
    public BezierCurve curve;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Shape profileShape = new Shape();

    // Extrusion parameters
    public AnimationCurve profile;
    public int numDivs = 10;
    public int numDivsProfile = 12;
    public float width = 10.0f;
    public float verticalScale = 10.0f;
    public bool generateCollider = true;

    void Start ()
    {
        curve = curve == null ? GetComponent<BezierCurve>() : curve;
        meshFilter = GetComponent<MeshFilter>();
        RegenerateProfileShape();
        RegenerateMesh();
    }

    void Update()
    {
        RegenerateProfileShape();
        RegenerateMesh();
    }

    /// <summary>
    /// Regenerates the profile shape to be extruded.
    /// </summary>
    public void RegenerateProfileShape()
    {
        Vector2[] points = new Vector2[numDivsProfile + 1];
        float[] uCoords = new float[numDivsProfile + 1];
        Vector2[] normals = new Vector2[numDivsProfile + 1];

        for (int i = 0; i < numDivsProfile + 1; ++i)
        {
            points[i].x = (float)i * (width / numDivsProfile);
            points[i].y = -profile.Evaluate(1.0f - Mathf.InverseLerp(0.0f, width, points[i].x)) * verticalScale;

            normals[i].x = 0.0f;
            normals[i].y = 1.0f;

            uCoords[i] = Mathf.InverseLerp(0.0f, width, points[i].x);
        }

        int[] lines = new int[points.Length * 2 - 2];
        int k = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            lines[k] = i;
            lines[k + 1] = i + 1;
            k += 2;
        }

        profileShape.points = points;
        profileShape.normals = normals;
        profileShape.uCoords = uCoords;
        profileShape.lines = lines;
    }
    
    public void RegenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] pts = {
                            (curve.a.localPosition),
                            (curve.b.localPosition),
                            (curve.c.localPosition),
                            (curve.d.localPosition)
                        };

        OrientedPoint[] path = new OrientedPoint[numDivs + 1];

        for (int i = 0; i < numDivs + 1; i++)
        {
            OrientedPoint p;
            float t = ((float)i / (float)numDivs);
            p.position = BezierCurve.GetPoint(pts, t);
            p.rotation = BezierCurve.GetOrientation3D(pts, (float)i / (float)numDivs, -Vector3.Lerp(curve.a.transform.up, curve.c.transform.up, t));

            path[i] = p;
        }

        Shape.Extrude(mesh, profileShape, path);

        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;

        if (generateCollider)
        {
            MeshCollider mc = GetComponent<MeshCollider>();
            if (mc == null)
            {
                mc = gameObject.AddComponent<MeshCollider>();
            }
            mc.sharedMesh = mesh;
        }

        Debug.Log("Mesh regenerated with vertex count: " + mesh.vertexCount);
    }
}
