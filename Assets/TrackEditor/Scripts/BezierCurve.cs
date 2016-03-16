using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BezierCurve : MonoBehaviour {

    static Material mat;

    public Transform a, b, c, d;

    public int numDivs = 10;


    public Vector2[] points = new Vector2[] {
        new Vector2(1.0f, 0.0f),
        new Vector2(2.0f, 0.0f),
        new Vector2(3.0f, 0.0f),
        new Vector2(4.0f, 0.0f),
        new Vector2(5.0f, 0.0f),
        new Vector2(6.0f, 0.0f)
    };

    public Vector2[] normals = new Vector2[] {
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 0.0f)
    };

    public float[] uCoords = new float[] {
        0.1f,
        0.2f,
        0.3f,
        0.4f,
        0.5f,
        0.6f
    };

    public int[] lines = new int[] {
        0, 1,
        1, 2,
        2, 3,
        3, 4,
        4, 5
    };

	// Use this for initialization
	void Start () 
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
            b.transform.parent = transform;
            b.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        } 
        if (c == null)
        {
            c = new GameObject("C").transform;
            c.transform.parent = transform;
            c.transform.localPosition = new Vector3(10.0f, 0.0f, 3.0f);
        }
        if (d == null)
        {
            d = new GameObject("D").transform;
            d.transform.parent = transform;
            d.transform.localPosition = new Vector3(10.0f, 0.0f, 0.0f);
        }
        RegenerateMesh();
	}

    void Update()
    {
        RegenerateMesh();
    }

    public void RegenerateMesh()
    {
        Shape s = new Shape();
        s.points = points;
        s.normals = normals;
        s.uCoords = uCoords;
        s.lines = lines;


        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            Debug.Log("Mesh was null");
        }

        Vector3[] pts = { a.localPosition, (b.localPosition), c.localPosition, d.localPosition };

        //Debug.Log(name + " " + a.InverseTransformPoint(b.position));

        OrientedPoint[] path = new OrientedPoint[numDivs + 1];

        for (int i = 0; i < numDivs+ 1; i++)
        {

            OrientedPoint p;
            p.position = GetPoint(pts, ((float)i / (float)numDivs));
            p.rotation = GetOrientation3D(pts, (float)i / (float)numDivs, -a.transform.up);

            //path[i] = p;
        }



        Shape.Extrude(mesh, s, path);

        Debug.Log("Mesh regenerated " + mesh.vertexCount);
        GetComponent<MeshFilter>().sharedMesh = mesh;   
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
