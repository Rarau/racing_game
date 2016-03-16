using UnityEngine;
using System.Collections;
using UnityEditor;

//[InitializeOnLoad]
//public class Autorun
//{
//    static Autorun()
//    {
//        EditorApplication.update += RunOnce;
//    }

//    static void RunOnce()
//    {
//        Debug.Log("RunOnce!");
//        EditorApplication.update -= RunOnce;

//        // This code will execute only once when editor is loaded.
//        //Curve[] curves = GameObject.FindObjectsOfType<Curve>();
//        //foreach (Curve c in curves)
//        //{
//        //    c.Load();
//        //}
//    }
//}

[CustomEditor(typeof(Curve))]
public class CurveEditor : Editor {

	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Curve myCurve = (Curve)target;
        if(GUILayout.Button("AddSpline"))
        {
            myCurve.AddSpline();
        }
        if (GUILayout.Button("DeleteLastSpline"))
        {
            myCurve.DeleteSpline();
        }
        if (GUILayout.Button("CloseCurve"))
        {
            myCurve.CloseCurve();
        }
        if (GUILayout.Button("ReCreateGeometry"))
        {
            myCurve.Extrude();
        }
        if (GUILayout.Button("ClearCurve"))
        {
            myCurve.ClearCurve();
        }
        //if (GUILayout.Button("Save"))
        //{
        //    myCurve.Save();
        //}
        //if (GUILayout.Button("Load"))
        //{
        //    myCurve.Load();
        //}
    }

}
