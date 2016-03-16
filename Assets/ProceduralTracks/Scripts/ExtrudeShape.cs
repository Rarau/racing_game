using UnityEngine;
using System.Collections;

public class ExtrudeShape
{
    public Vector2[] verts;
    public Vector2[] curvedVerts;

    public Vector2[] normals;
    public Vector2[] curvedNormals;

    public float[] us;
    public int[] lines;

    public void Initialize(float trackWidth, int divisions)
    {
        // Calculate verts
        verts = new Vector2[divisions + 1 + 1];
        for (int i = 0; i < divisions + 1; ++i)
        {
            verts[i] = new Vector2(trackWidth / divisions * i, 0);
        }
        verts[divisions + 1] = new Vector2(trackWidth, 0.25f);

        // Calculate normals
        normals = new Vector2[divisions + 1 + 1];
        for (int i = 0; i < divisions + 1; ++i)
        {
            normals[i] = new Vector2(0, 1);
        }
        normals[divisions + 1] = new Vector2(-1, 0);

        // Calculate curved verts
        curvedVerts = new Vector2[divisions + 1 + 1];
        float angle = Mathf.PI / 2;
        for (int i = 0; i < divisions + 1; ++i)
        {
            // circle formula: (x-c1)^2 + (y-c2)^2 = r^2
            float x = trackWidth * Mathf.Cos(angle);
            float y = trackWidth - trackWidth * Mathf.Sin(angle);
            curvedVerts[i] = new Vector2(x, y);
            angle -= (Mathf.PI / 2) / divisions;
        }
        curvedVerts[divisions + 1] = curvedVerts[divisions] + new Vector2(0, 0.25f);

        //curvedVerts = new Vector2[divisions + 1];
        //for (int i = 0; i < divisions + 1; ++i)
        //{
        //    // circle formula: (x-c1)^2 + (y-c2)^2 = r^2
        //    float x = trackWidth / divisions * i;
        //    float y = -Mathf.Sqrt(trackWidth * trackWidth - x * x) + trackWidth;
        //    curvedVerts[i] = new Vector2(x, y);
        //}

        // Calculate curved normals
        curvedNormals = new Vector2[divisions + 1 + 1];
        angle = 0;
        for (int i = 0; i < divisions + 1; ++i)
        {
            curvedNormals[i] = (new Vector2(0, trackWidth)) - curvedVerts[i];
            angle += (Mathf.PI / 2) / divisions;
        }
        curvedNormals[divisions + 1] = new Vector2(-1, 0);

        // Calculate us
        us = new float[verts.Length];
        for (int i = 0; i < verts.Length; ++i)
        {
            us[i] = i * (1.0f / ((float)verts.Length - 1));
        }

        // Calculate lines
        lines = new int[(verts.Length - 1) * 2];
        int counter = 0;
        for (int i = 0; i < divisions + 1; ++i)
        {
            //if (i < lines.Length)
            {
                lines[i * 2] = counter;
                lines[i * 2 + 1] = counter + 1;
                ++counter;
            }
        }
    }





}

