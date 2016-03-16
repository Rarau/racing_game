using UnityEditor;
using UnityEngine;
using System.IO;

public class MenuExtension : MonoBehaviour
{   
    [MenuItem("Track/AddCurve")]
    static void AddCurve()
    {
        GameObject track = GameObject.Find("ProceduralTrack");
        if (track == null)
        {
            track = new GameObject("ProceduralTrack");            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(track, "Create " + track.name);
            Selection.activeObject = track;
            track.AddComponent<Track>();
            MeshRenderer mR = track.AddComponent<MeshRenderer>();            

        }
        Track trackScript = track.GetComponent<Track>();


        if (File.Exists(Application.dataPath + "/CurvesSavedData/" + "curve" + trackScript.curveIdGenerator + ".curve"))
        {
            File.Delete(Application.dataPath + "/CurvesSavedData/" + "curve" + trackScript.curveIdGenerator + ".curve");
        }
        GameObject curvePrefab = (GameObject)Resources.Load("CurvePrefab");
        GameObject go = Instantiate(curvePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.name = "curve" + trackScript.curveIdGenerator;
        ++trackScript.curveIdGenerator;
        trackScript.Save();
        go.transform.parent = track.transform;   
             

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("Track/AddBifurcation")]
    static void AddBifurcation()
    {
        GameObject track = GameObject.Find("ProceduralTrack");
        if (track == null)
        {
            track = new GameObject("ProceduralTrack");
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(track, "Create " + track.name);
            Selection.activeObject = track;
            track.AddComponent<Track>();
            track.AddComponent<MeshRenderer>();
            track.AddComponent<MeshCollider>();
        }        
        Track trackScript = track.GetComponent<Track>();


        if (File.Exists(Application.dataPath + "/CurvesSavedData/" + "bifurcation" + trackScript.bifIdGenerator + ".curve"))
        {
            File.Delete(Application.dataPath + "/CurvesSavedData/" + "bifurcation" + trackScript.bifIdGenerator + ".curve");
        }
        GameObject bifurcationPrefab = (GameObject)Resources.Load("BifurcationPrefab");
        GameObject go = Instantiate(bifurcationPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.name = "bifurcation" + trackScript.bifIdGenerator;
        ++trackScript.bifIdGenerator;
        trackScript.Save();
        go.transform.parent = track.transform;


        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
