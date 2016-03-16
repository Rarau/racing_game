using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct BezierData
{

}

[ExecuteInEditMode]
public class BezierSpline : MonoBehaviour
{    
    public Material material;
    public TrackElement curve;

    public int numTracersDebug = 100;

    public Node startNode;
    public Node endNode;

    OrientedPoint[] orientedPoints;    

    public Vector3 GetPoint(float t)
    {
        if (endNode == null) return Vector3.zero;

        Vector3 p0 = startNode.position;
        Vector3 p1 = startNode.frontControl;
        Vector3 p2 = endNode.backControl;
        Vector3 p3 = endNode.position;

        /*
            L1 = Lerp(start-startControl) , L2 = Lerp(startControl-endControl) , L3 = Lerp(endControl-end)
            L1' = Lerp(L1-L2) , L2' = Lerp(L2-L3)
            result = Lerp(L1'-L2')

            We use here the bernstein polynomial form of this algorithm for performance issues
        */

        Vector3 result =
            p0 * ((1 - t) * (1 - t) * (1 - t)) +
            p1 * (3 * (1 - t) * (1 - t) * t) +
            p2 * (3 * (1 - t) * t * t) +
            p3 * (t * t * t);

        return result;

    }

    public Vector3 GetTangent(float t)
    {
        if (endNode == null) return Vector3.zero;

        Vector3 p0 = startNode.position;
        Vector3 p1 = startNode.frontControl;
        Vector3 p2 = endNode.backControl;
        Vector3 p3 = endNode.position;

        // Vector between L1' and L2', see GetPoint

        Vector3 result =
            p0 * (-t * t + 2 * t - 1) +
            p1 * (3 * t * t - 4 * t + 1) +
            p2 * (-3 * t * t + 2 * t) +
            p3 * (t * t);

        return result.normalized;

    }

    public Vector3 GetNormal(float t, Vector3 up)
    {
        if (endNode == null) return Vector3.zero;

        Vector3 tangent = GetTangent(t);
        Vector3 binormal = Vector3.Cross(up, tangent);
        return Vector3.Cross(tangent, binormal).normalized;

    }

    public Vector3 GetBinormal(float t, Vector3 up)
    {
        if (endNode == null) return Vector3.zero;

        Vector3 tangent = GetTangent(t);
        return Vector3.Cross(up, tangent);
    }

    public Quaternion GetOrientation(float t, Vector3 up)
    {
        if (endNode == null) return Quaternion.identity;

        Vector3 tangent = GetTangent(t);
        Vector3 normal = GetNormal(t, up);

        return Quaternion.LookRotation(tangent, normal);
    }

    void OnDrawGizmos()
    {
        if (endNode == null) return;

        float segmentLength = 1.0f / (float)numTracersDebug;
        Vector3 start = startNode.position;
        for (int i = 1; i < numTracersDebug+1; ++i)
        {
            Vector3 end = GetPoint(i * segmentLength);
            Gizmos.DrawLine(start, end);

            //if(i%20 == 0)
            //{
            //    DebugExtension.DebugArrow(start, GetTangent(i * segmentLength), Color.cyan);

            //    Vector3 up = Vector3.Lerp(startNode.transform.up, endNode.transform.up, i * segmentLength);
            //    DebugExtension.DebugArrow(start, GetNormal(i * segmentLength, up), Color.green);
            //    DebugExtension.DebugArrow(start, GetBinormal(i * segmentLength, up), Color.red);
            //}          

            

            start = end;
        }
        //if (orientedPoints != null)
        //{
        //    for (int j = 0; j < orientedPoints.Length; ++j)
        //    {
        //        Gizmos.DrawWireSphere(transform.TransformPoint(orientedPoints[j].position), 0.1f);
        //    }
        //} 
    }

    // find the length of the spline
    float GetLength()
    {
        // chorded approximation
        float length = 0;

        int numChords = 100;
        float chordLength = 1.0f / (float)numChords;
        Vector3 start = startNode.position;
        for (int i = 1; i < numChords + 1; ++i)
        {
            Vector3 end = GetPoint(i * chordLength);
            length += Vector3.Distance(start, end);            

            start = end;
        }
        return length;
    }

    public void Extrude(Mesh mesh, ExtrudeShape shape)
    {
        if (shape == null) shape = new ExtrudeShape();

        float splineLength = GetLength();
        shape.Initialize(1, curve.horizontalDivisions);        

        int divisions = (int)(curve.divisionsPerCurve * splineLength / curve.trackWidth / 20);
        if (divisions < 1) divisions = 1;
        int vertsInShape = shape.verts.Length;

        List<int> triangleIndices = new List<int>(); //new int[triIndexCount];
        List <Vector3> vertices = new List<Vector3>();  //new Vector3[vertCount];
        List<Vector3> normals = new List<Vector3>(); //new Vector3[vertCount];
        List<Vector2> uvs = new List<Vector2>(); //new Vector2[vertCount];

        /*
            Mesh generation code
        */
        #region meshGeneration

        float divisionLength = 1.0f / (float)divisions;
        orientedPoints = new OrientedPoint[divisions + 1];

        #region rightSide
        int rightOffset = 0;

        // Create vertices
        for (int i = 0; i <= divisions; ++i)
        {// for each edgeLoop
            float t = i * divisionLength;

            Vector3 up = Vector3.Lerp(startNode.transform.up, endNode.transform.up, t);
            float curvature = Mathf.Lerp(startNode.rightCurvature, endNode.rightCurvature, t);
            float width = (curve.trackWidth + Mathf.Lerp(startNode.trackWidthModifier, endNode.trackWidthModifier, t))/2;
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(width, width, width));            

            // Initialize oriented point
            orientedPoints[i].position = transform.InverseTransformPoint(GetPoint(t));
            orientedPoints[i].rotation =  Quaternion.Inverse(transform.rotation) * (GetOrientation(t, up));
            orientedPoints[i].scale = scale;

            for (int j = 0; j < vertsInShape; ++j)
            {// for each vertex in the shape

                Vector2 vertex = Vector2.Lerp(shape.verts[j], shape.curvedVerts[j], curvature);
                Vector2 normal = Vector2.Lerp(shape.normals[j], shape.curvedNormals[j], curvature);

                vertices.Add(orientedPoints[i].LocalToWorld(vertex));
                normals.Add(orientedPoints[i].LocalToWorldDirection(normal));

                // u is based on the 2D shape, and v is based on the distance along the curve
                uvs.Add(new Vector2(shape.us[j], t * splineLength / width));                
            }            
        }

        // Create triangles
        for (int j = 0; j < divisions; ++j)
        {// for each division in the curve
            int offset = shape.verts.Length * j;

            for(int k = 0; k < shape.lines.Length; k = k+2)
            {//for each 2d line in the shape
                // triangle 1                
                triangleIndices.Add(shape.lines[k + 1] + offset);
                triangleIndices.Add(shape.lines[k] + offset);
                triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                // triangle 2
                triangleIndices.Add(shape.lines[k + 1] + offset);
                triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                triangleIndices.Add(shape.lines[k + 1] + offset + shape.verts.Length);
                                
            }

        }
        rightOffset = vertices.Count;
        #endregion

        #region leftSide
        for (int i = 0; i <= divisions; ++i)
        {// for each edgeLoop
            float t = i * divisionLength;

            Vector3 up = Vector3.Lerp(startNode.transform.up, endNode.transform.up, t);
            float curvature = Mathf.Lerp(startNode.leftCurvature, endNode.leftCurvature, t);
            float width = curve.trackWidth + Mathf.Lerp(startNode.trackWidthModifier, endNode.trackWidthModifier, t);

            // Initialize oriented point
            orientedPoints[i].position = transform.InverseTransformPoint(GetPoint(t));
            orientedPoints[i].rotation = Quaternion.Inverse(transform.rotation) * (GetOrientation(t, up));

            for (int j = 0; j < vertsInShape; ++j)
            {// for each vertex in the shape

                Vector2 vertex = Vector2.Lerp(shape.verts[j], shape.curvedVerts[j], curvature);
                vertex.x *= -1;
                Vector2 normal = Vector2.Lerp(shape.normals[j], shape.curvedNormals[j], curvature);
                

                vertices.Add(orientedPoints[i].LocalToWorld(vertex));
                normals.Add(orientedPoints[i].LocalToWorldDirection(normal));

                // u is based on the 2D shape, and v is based on the distance along the curve
                uvs.Add(new Vector2(shape.us[j], t * splineLength / width));
            }
        }
        #endregion

        // Create triangles
        for (int j = 0; j < divisions; ++j)
        {// for each division in the curve
            int offset = shape.verts.Length * j;           

            for (int k = 0; k < shape.lines.Length; k = k + 2)
            {//for each 2d line in the shape
                // triangle 1
                triangleIndices.Add(shape.lines[k] + offset + rightOffset);
                triangleIndices.Add(shape.lines[k + 1] + offset + rightOffset);
                triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length + rightOffset);
                // triangle 2                
                triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length + rightOffset);
                triangleIndices.Add(shape.lines[k + 1] + offset + rightOffset);
                triangleIndices.Add(shape.lines[k + 1] + offset + shape.verts.Length + rightOffset);

            }

        }
        #endregion

        if (mesh == null) mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        //mesh.RecalculateNormals();      

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    public Vector3 splineCoordsToNode(Vector3 v)
    {
        return startNode.transform.InverseTransformPoint(transform.TransformPoint(v));
    }

    public void ExtrudeSide(Mesh mesh, ExtrudeShape shape, string side, float planeX)
    {
        Vector3 position = new Vector3(planeX, 0, 0);
        Vector3 normal = new Vector3(1, 0, 0);

        Extrude(mesh, shape);
        mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

        int[] triangleIndices = mesh.GetIndices(0); //new int[triIndexCount]; 
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;


        List<Vector2> uvs = new List<Vector2>();
        mesh.GetUVs(0, uvs); //new Vector2[vertCount];

        List < Vector3 > newVertices = new List<Vector3>();
        List<Vector3> newNormals = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        List<int> newIndices = new List<int>();

        // We will cut the mesh with a plane.
        // for each triangle (looking at indices) process if one vertex on the other side of the plane
        for (int i = 0; i < triangleIndices.Length; i = i + 3)
        {
            List<int> side1 = new List<int>();
            List<int> side2 = new List<int>();

            
            if (splineCoordsToNode(vertices[triangleIndices[i]]).x > planeX) side2.Add(triangleIndices[i]);
            else side1.Add(triangleIndices[i]);
            if (splineCoordsToNode(vertices[triangleIndices[i + 1]]).x > planeX) side2.Add(triangleIndices[i + 1]);
            else side1.Add(triangleIndices[i + 1]);
            if (splineCoordsToNode(vertices[triangleIndices[i + 2]]).x > planeX) side2.Add(triangleIndices[i + 2]);
            else side1.Add(triangleIndices[i + 2]);
           
            

            // Depending on the side evaluated determine wich vertices are in and which out
            List<int> inside;
            List<int> outside;
            if (side == "left")
            {// left
                inside = side1;
                outside = side2;
            }
            else
            {// right
                inside = side2;
                outside = side1;
            }

            // Cases
            if (inside.Count == 3)
            {// all vertices on the correct side
                continue;
            }
            else
            {
                // discard triangle
                triangleIndices[i] = triangleIndices[i + 1] = triangleIndices[i + 2] = 0;
                
                if(outside.Count == 2)
                {// case 1: 1 vertices in, 2 out. Create 2 more vertices and one tri.

                    Vector3 v1 = Intersection(vertices[inside[0]], vertices[outside[0]], normal, position);
                    Vector3 v2 = Intersection(vertices[inside[0]], vertices[outside[1]], normal, position);
                    newVertices.Add(v1);
                    newVertices.Add(v2);

                    newNormals.Add(new Vector3(0, 1, 0));
                    newNormals.Add(new Vector3(0, 1, 0));

                    newUVs.Add(new Vector3(0, 0, 0));
                    newUVs.Add(new Vector3(0, 0, 0));

                    if (side == "right" || (vertices[outside[0]].z > vertices[outside[1]].z))
                    {
                        newIndices.Add(inside[0]);
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v1));
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));
                    }
                    else
                    {
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v1));
                        newIndices.Add(inside[0]);                        
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));                        
                    }
                }
                else if(outside.Count == 1)
                {// case 2: 2 vertices in, 1 out. create 2 more vertices and 2 tris.

                    Vector3 v1 = Intersection(vertices[inside[0]], vertices[outside[0]], normal, position);
                    Vector3 v2 = Intersection(vertices[inside[1]], vertices[outside[0]], normal, position);
                    newVertices.Add(v1);
                    newVertices.Add(v2);

                    newNormals.Add(new Vector3(0, 1, 0));
                    newNormals.Add(new Vector3(0, 1, 0));

                    newUVs.Add(new Vector3(0, 0, 0));
                    newUVs.Add(new Vector3(0, 0, 0));

                    if (side == "right" || (vertices[inside[0]].z > vertices[inside[1]].z))
                    {
                        newIndices.Add(inside[0]);
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v1));
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));

                        newIndices.Add(inside[1]);
                        newIndices.Add(inside[0]);
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));
                    }
                    else
                    {
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v1));
                        newIndices.Add(inside[0]);                        
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));

                        newIndices.Add(inside[0]);
                        newIndices.Add(inside[1]);
                        newIndices.Add(vertices.Length + newVertices.IndexOf(v2));
                    }
                }
            }
            
        }

        vertices = Concat<Vector3>(vertices, newVertices.ToArray());
        normals = Concat<Vector3>(normals, newNormals.ToArray());
        Vector2[] muvs = Concat<Vector2>(uvs.ToArray(), newUVs.ToArray());
        triangleIndices = Concat<int>(triangleIndices, newIndices.ToArray());

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = muvs;

        //mesh.RecalculateNormals();      

        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public T[] Concat<T>(T[] x, T[] y)
    {
        var z = new T[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);
        return z;
    }

    public Vector3 Intersection(Vector3 p1, Vector3 p2, Vector3 normal, Vector3 planePoint)
    {        
        p1 = startNode.transform.InverseTransformPoint(transform.TransformPoint(p1));
        p2 = startNode.transform.InverseTransformPoint(transform.TransformPoint(p2));

        float u = (Vector3.Dot(normal, planePoint - p1)) / (Vector3.Dot(normal, p2 - p1));

        Vector3 point = p1 + u*(p2 - p1);
        point = transform.InverseTransformPoint(startNode.transform.TransformPoint(point));
        return point;
    }


    public void ExtrudeSideOld(Mesh mesh, ExtrudeShape shape, string side)
    {
        if (shape == null) shape = new ExtrudeShape();

        int factor = 1;
        if (side == "left") factor = -1;
        

        float splineLength = GetLength();
        shape.Initialize(1, curve.horizontalDivisions);

        int divisions = (int)(curve.divisionsPerCurve * splineLength / curve.trackWidth / 20);
        if (divisions < 1) divisions = 1;
        int vertsInShape = shape.verts.Length;

        List<int> triangleIndices = new List<int>(); //new int[triIndexCount];
        List<Vector3> vertices = new List<Vector3>();  //new Vector3[vertCount];
        List<Vector3> normals = new List<Vector3>(); //new Vector3[vertCount];
        List<Vector2> uvs = new List<Vector2>(); //new Vector2[vertCount];

        /*
            Mesh generation code
        */
        #region meshGeneration

        float divisionLength = 1.0f / (float)divisions;
        orientedPoints = new OrientedPoint[divisions + 1];

        // Create vertices
        for (int i = 0; i <= divisions; ++i)
        {// for each edgeLoop
            float t = i * divisionLength;

            Vector3 up = Vector3.Lerp(startNode.transform.up, endNode.transform.up, t);
            float curvature = Mathf.Lerp(startNode.rightCurvature, endNode.rightCurvature, t);
            float width = (curve.trackWidth + Mathf.Lerp(startNode.trackWidthModifier, endNode.trackWidthModifier, t)) / 2;
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(width, width, width));

            // Initialize oriented point
            orientedPoints[i].position = transform.InverseTransformPoint(GetPoint(t));
            orientedPoints[i].rotation = Quaternion.Inverse(transform.rotation) * (GetOrientation(t, up));
            orientedPoints[i].scale = scale;

            for (int j = 0; j < vertsInShape; ++j)
            {// for each vertex in the shape

                Vector2 vertex = Vector2.Lerp(shape.verts[j], shape.curvedVerts[j], curvature);
                vertex.x *= factor;
                Vector2 normal = Vector2.Lerp(shape.normals[j], shape.curvedNormals[j], curvature);

                vertices.Add(orientedPoints[i].LocalToWorld(vertex));
                normals.Add(orientedPoints[i].LocalToWorldDirection(normal));

                // u is based on the 2D shape, and v is based on the distance along the curve
                uvs.Add(new Vector2(shape.us[j], t * splineLength / width));
            }
        }

        // Create triangles
        for (int j = 0; j < divisions; ++j)
        {// for each division in the curve
            int offset = shape.verts.Length * j;

            for (int k = 0; k < shape.lines.Length; k = k + 2)
            {//for each 2d line in the shape
                if (side == "right")
                {
                    // triangle 1                
                    triangleIndices.Add(shape.lines[k + 1] + offset);
                    triangleIndices.Add(shape.lines[k] + offset);
                    triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                    // triangle 2
                    triangleIndices.Add(shape.lines[k + 1] + offset);
                    triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                    triangleIndices.Add(shape.lines[k + 1] + offset + shape.verts.Length);
                }
                else
                {
                    // triangle 1     
                    triangleIndices.Add(shape.lines[k] + offset);
                    triangleIndices.Add(shape.lines[k + 1] + offset);                    
                    triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                    // triangle 2
                    triangleIndices.Add(shape.lines[k] + offset + shape.verts.Length);
                    triangleIndices.Add(shape.lines[k + 1] + offset);                    
                    triangleIndices.Add(shape.lines[k + 1] + offset + shape.verts.Length);
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        #endregion

        mesh.RecalculateNormals();      

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Split()
    {
        Quaternion orientation = GetOrientation(0.5f, Vector3.Lerp(startNode.transform.up, endNode.transform.up, 0.5f));
        Vector3 position = GetPoint(0.5f);
        
        // new intermediate node creation, taking care of inserting it in the correct position    
        //Node newNode = curve.CreateNode(position, orientation);
        GameObject nodeGO = Instantiate(curve.nodePrefab, position, orientation) as GameObject;
        nodeGO.transform.parent = curve.transform;
        Node node = nodeGO.GetComponent<Node>();

        node.position = position;
        node.curve = this.curve;

        int index = curve.nodes.IndexOf(endNode);
        curve.nodes.Insert(index, node);

        // we need to create 2 splines now        
        curve.CreateSpline(this.startNode, node);
        curve.CreateSpline(node, endNode);

        // Make sure that its removed from the curve list and eliminate the associated mesh
        curve.meshes.RemoveAt(curve.splines.IndexOf(this));
        curve.splines.Remove(this);        
        DestroyImmediate(this.gameObject);
    }

}
