using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BezierCurve : MonoBehaviour 
{
    Shape profileShape = new Shape();

    static Material mat;

    [HideInInspector]
    public Transform a, b, c, d;

    public int numDivs = 10;
    public int numDivsProfile = 12;
    public float width = 10.0f;
    public float verticalScale = 10.0f;

    public AnimationCurve profile;

    public BezierCurve nextCurve;

    //public Vector3[] controlPoints;
    /*
    Vector2[] points = new Vector2[] {
        new Vector2(1.0f, 0.0f),
        new Vector2(2.0f, 0.0f),
        new Vector2(3.0f, 0.0f),
        new Vector2(4.0f, 0.0f),
        new Vector2(5.0f, 0.0f),
        new Vector2(6.0f, 0.0f)
    };

    Vector2[] normals = new Vector2[] {
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f)
    };

    float[] uCoords = new float[] {
        0.1f,
        0.2f,
        0.3f,
        0.4f,
        0.5f,
        0.6f
    };

    int[] lines = new int[] {
        0, 1,
        1, 2,
        2, 3,
        3, 4,
        4, 5
    };
    */
	// Use this for initialization
	public void Start() 
    {
        if (a == null)
        {
            a = new GameObject("A").transform;
            a.transform.parent = transform;
            a.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        if (b == null)
        {
            b = new GameObject("B").transform;
            b.transform.parent = a;
            b.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        } 

        if (d == null)
        {
            d = new GameObject("D").transform;
            d.transform.parent = transform;
            d.transform.localPosition = new Vector3(10.0f, 0.0f, 0.0f);
        }

        if (c == null)
        {
            c = new GameObject("C").transform;
            c.transform.parent = d;
            c.transform.localPosition = new Vector3(10.0f, 0.0f, 3.0f);
        }

        profileShape = new Shape();

        RegenerateProfileShape();
        RegenerateMesh();
	}

    void Update()
    {
       // RegenerateProfileShape();
       // RegenerateMesh();

        if (nextCurve != null)
        {
           nextCurve.a.transform.position = d.transform.position;
           nextCurve.a.transform.rotation = d.transform.rotation;

        }
    }

    /// <summary>
    /// Regenerates the profile shape to be extruded.
    /// </summary>
    public void RegenerateProfileShape()
    {
        Vector2[] points = new Vector2[numDivsProfile + 1];
        float[] uCoords = new float[numDivsProfile + 1];
        Vector2[] normals = new Vector2[numDivsProfile + 1];

        for(int i = 0; i < numDivsProfile + 1; ++i)
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
                            transform.InverseTransformPoint(a.position),
                            transform.InverseTransformPoint(b.position),
                            transform.InverseTransformPoint(c.position),
                            transform.InverseTransformPoint(d.position)
                        };

        //Debug.Log(name + " " + a.InverseTransformPoint(b.position));

        OrientedPoint[] path = new OrientedPoint[numDivs + 1];

        for (int i = 0; i < numDivs+ 1; i++)
        {
            OrientedPoint p;
            float t = ((float)i / (float)numDivs);
            p.position = GetPoint(pts, t);
            p.rotation = GetOrientation3D(pts, (float)i / (float)numDivs, -Vector3.Lerp(a.transform.up, c.transform.up, t));

            path[i] = p;
        }

        Shape.Extrude(mesh, profileShape, path);
        
        mesh.RecalculateNormals();

        Debug.Log("Mesh regenerated " + mesh.vertexCount);
        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshCollider mc = GetComponent<MeshCollider>();
        if(mc == null)
        {
            mc = gameObject.AddComponent<MeshCollider>();
        }
        mc.sharedMesh = mesh;
    }
    
    static void CreateLineMaterial()
    {
        if (!mat)
        {
            mat = new Material(Shader.Find("Unlit/Color"));
        }
    }

	// Update is called once per frame 
    public void OnDrawGizmos()
    {
        if (nextCurve != null)
        {
            //nextCurve.Update();
            d.transform.position = nextCurve.a.transform.position;
            d.transform.rotation = nextCurve.a.transform.rotation;

        }
        
        CreateLineMaterial();

        Vector3[] pts = { a.position, b.position, c.position, d.position };

        //GL.PushMatrix();
        //GL.LoadIdentity();
        //GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);    

        GL.Color(Color.yellow);   
        mat.SetPass(0);
      
        for (int i = 0; i < numDivs; i++)
        {

            GL.Vertex(GetPoint(pts, ((float)i / (float)numDivs)));
            GL.Vertex(GetPoint(pts, ((float)(i + 1) / (float)numDivs)));


        }

        Debug.DrawLine(a.position, b.position, Color.white);
        Debug.DrawLine(c.position, d.position, Color.white);
        GL.End();

        //GL.PopMatrix();
         
	}

    Vector3 GetPoint(Vector3[] pts, float t)
    {
        float omt = 1.0f - t;
        float omt2 = omt * omt;
        float t2 = t * t;

        return pts[0] * (omt2 * omt) +
            pts[1] * (3.0f * omt2 * t) +
            pts[2] * (3.0f * omt * t2) +
            pts[3] * (t2 * t);
    }

    Vector3 GetTangent(Vector3[] pts, float t)
    {
        float omt = 1.0f - t;
        float omt2 = omt * omt;
        float t2 = t * t;

        Vector3 tangent =
            pts[0] * (-omt2) +
            pts[1] * (3.0f * omt2 - 2.0f * omt) +
            pts[2] * ( -3.0f * t2 + 2 * t) +
            pts[3] * (t2);

        return tangent.normalized;
    }

    Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }

    Quaternion GetOrientation3D(Vector3[] pts, float t, Vector3 up)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 nrm = GetNormal3D(pts, t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

}
