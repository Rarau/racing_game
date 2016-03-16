using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class TrackData
{
    public int curveId;
    public int bifId;

    public float trackWidth;
    public int horizontalDivisions;
    public int divisionsPerCurve = 5;
}

[ExecuteInEditMode]
public class Track : MonoBehaviour
{
    public float trackWidth = 2;
    public int horizontalDivisions = 10;
    public int divisionsPerCurve = 20;

    public int curveIdGenerator = 0;
    public int bifIdGenerator = 0;

    public Mesh mesh;

    string lastState = "";
    void Update()
    {
        // Reload when changing between editor and play modes
        string state = "";
        if (EditorApplication.isPlaying)
            state = "PlayMode";
        else
        {
            state = "EditorMode";            
        }
        if (state != lastState)
        {
            Load();
            if (state == "PlayMode") CombineMeshes();
            if (state == "EditorMode") ReactivateMeshes();
        }
        lastState = state;

        // On editor: save changes and recreate geometry
        if (state == "EditorMode")
        {
            Save();
        }
    }

    public void ReactivateMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();        
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].gameObject != gameObject)
            {
                meshFilters[i].gameObject.SetActive(true);
            }
            i++;
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].gameObject != gameObject)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = transform.worldToLocalMatrix * (meshFilters[i].transform.localToWorldMatrix);
                meshFilters[i].gameObject.SetActive(false);                
            }
            i++;
        }
        if (gameObject.GetComponent<MeshFilter>() == null)
                    gameObject.AddComponent<MeshFilter>();
        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        if (gameObject.GetComponent<MeshCollider>() == null)
            gameObject.AddComponent<MeshCollider>();

         
        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;
    }

    public void Save()
    {
        //Debug.Log("Track ids saved");

        TrackElement[] children = transform.GetComponentsInChildren<TrackElement>();
        foreach (TrackElement e in children)
        {
            e.trackWidth = trackWidth;
            e.horizontalDivisions = horizontalDivisions;
            e.divisionsPerCurve = divisionsPerCurve;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve");

        TrackData data = new TrackData();
        data.bifId = bifIdGenerator;
        data.curveId = curveIdGenerator;

        data.trackWidth = trackWidth;
        data.horizontalDivisions = horizontalDivisions;
        data.divisionsPerCurve = divisionsPerCurve;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve", FileMode.Open);
            TrackData data = (TrackData)bf.Deserialize(file);
            file.Close();

            curveIdGenerator = data.curveId;
            bifIdGenerator = data.bifId;

            trackWidth = data.trackWidth;
            horizontalDivisions = data.horizontalDivisions;
            divisionsPerCurve = data.divisionsPerCurve;
        }
    }

}


