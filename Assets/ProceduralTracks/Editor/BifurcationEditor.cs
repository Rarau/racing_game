using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Bifurcation))]
public class BifurcationEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Bifurcation bifurcation = (Bifurcation)target;
        if (GUILayout.Button("ReCreateGeometry"))
        {
            bifurcation.Load();
        }
        if (GUILayout.Button("Clear"))
        {
            bifurcation.ClearCurve();
        }

    }

}