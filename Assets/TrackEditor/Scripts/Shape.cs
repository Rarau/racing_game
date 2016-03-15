using UnityEngine;
using System.Collections;



public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;

    public OrientedPoint(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public Vector3 LocalToWorld(Vector3 point)
    {
        return position + rotation * point;
    }

    public Vector3 WorldToLocal(Vector3 point)
    {
        return Quaternion.Inverse(rotation) * (point - position);
    }

    public Vector3 LocalToWorldDirection(Vector3 dir)
    {
        return rotation * dir;
    }
}

public class Shape
{

    public Vector2[] points;
    public Vector2[] normals;
    public float[] uCoords;
    public int[] lines = new int[] {
        0, 1,
        2, 3,
        4, 5
    };

    public static void Extrude(Mesh mesh, Shape shape, OrientedPoint[] path)
    {
        int vertsInShape = shape.points.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = edgeLoops * vertsInShape;
        int triCount = segments * (vertsInShape - 1) * 2;
        int triIndexCount = triCount * 3;

        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

        mesh.Clear();

        for (int i = 0; i < path.Length; i++)
        {
            int offset = i * vertsInShape;
            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.points[j]);
                normals[id] = path[i].LocalToWorldDirection(shape.normals[j]);
                uvs[id] = new Vector2(shape.uCoords[j], i / ((float)edgeLoops));
            }
        }
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.lines.Length; l += 2)
            {
                int a = offset + shape.lines[l] + vertsInShape;
                int b = offset + shape.lines[l];
                int c = offset + shape.lines[l + 1];
                int d = offset + shape.lines[l + 1] + vertsInShape;
                triangleIndices[ti] = a; ti++;
                triangleIndices[ti] = b; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = d; ti++;
                triangleIndices[ti] = a; ti++;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangleIndices;
        mesh.uv = uvs;
    }
}
