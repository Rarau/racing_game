using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BezierSpline spline = (BezierSpline)target;
        if (GUILayout.Button("Split"))
        {
            spline.Split();
        }
        
    }

}