using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System;


[Serializable]
class CurveData
{
    public bool closed;
    public NodeData[] nodesData;
    //public BezierSpline[] splinesData;
}

[ExecuteInEditMode]
public class Curve : TrackElement
{
    // connectors
    public TrackElement nextCurve;
    // ----------
    
    bool closed = false;
    bool connected = false;
    
    string lastState = "";

    void Awake()
    {
        //if (!(EditorApplication.isPlaying))
        //{            
        //    if(splines.Count == 0) AddSpline();
        //    Save();
        //}
    }

    void Update()
    {
        // Reload curve when changing between editor and play modes
        string state = "";
        if (EditorApplication.isPlaying)
            state = "PlayMode";
        else
        {
            state = "EditorMode";            
            //Debug.Log("Saved");
        }
        if(state != lastState)
        {            
            Load();
        }
        lastState = state;

        // On editor: save changes and recreate geometry
        if (state == "EditorMode")
        {
            if (nodes != null) Save();
            if (splines != null) Extrude();
        }

        connected = false;
        // Maintain conection with next curve        
        if (nextCurve != null && nextCurve.nodes.Count > 0)
        {
            nodes[nodes.Count - 1].Copy(nextCurve.nodes[0]);
            connected = true;
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve");        

        CurveData data = new CurveData();

        List<NodeData> _nodesData = new List<NodeData>();
        if (nodes != null)
        {
            foreach (Node n in nodes)
            {
                _nodesData.Add(n.Serialize());
            }
        }
        data.nodesData = _nodesData.ToArray();

        //List<BezierSpline> _splinesData = new List<BezierSpline>();
        //foreach (BezierSpline b in splines)
        //{
        //    _splinesData.Add(b.Serialize());
        //}
        //data.splinesData = _splinesData.ToArray();

        data.closed = this.closed;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve"))
        {
            ClearCurve();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/ProceduralTracks/CurvesSavedData/" + gameObject.name + ".curve", FileMode.Open);
            CurveData data = (CurveData)bf.Deserialize(file);
            file.Close();

            if (extrudeShape == null) { extrudeShape = new ExtrudeShape(); }

            Node previousNode = null;
            for (int i = 0; i < data.nodesData.Length; ++i)
            {
                // Create Node                
                Node node = CreateNode(transform.position, transform.rotation);
                node.Load(data.nodesData[i]);

                // If not the first, create a spline with the previous one
                if (previousNode != null)
                {
                    BezierSpline spline = CreateSpline(previousNode, node);
                    spline.Extrude(meshes[i - 1], extrudeShape);
                }
                previousNode = node;
            }

            this.closed = data.closed;
            if(closed)
            {
                BezierSpline spline = CreateSpline(nodes[nodes.Count-1], nodes[0]);
                spline.Extrude(meshes[meshes.Count-1], extrudeShape);
            }
        }
        else
        {
            Debug.Assert(true, "Data file not found");
        }
    }

    public override Node CreateNode(Vector3 position, Quaternion rotation)
    {
        GameObject nodeGO = Instantiate(nodePrefab, position, rotation) as GameObject;
        nodeGO.transform.parent = transform;        
        Node node = nodeGO.GetComponent<Node>();

        node.frontTransform = node.transform.FindChild("front");
        node.backTransform = node.transform.FindChild("back");

        node.position = position;
        node.curve = this;
        nodes.Add(node);        

        return node;
    }

    public override BezierSpline CreateSpline(Node start, Node end)
    {
        GameObject splineGO = Instantiate(splinePrefab, transform.position, transform.rotation) as GameObject;
        splineGO.transform.parent = transform;
        BezierSpline spline = splineGO.GetComponent<BezierSpline>();

        spline.curve = this;
        spline.startNode = start;
        spline.endNode = end;

        spline.transform.position = (start.transform.position + end.transform.position) / 2;

        splines.Add(spline);
        meshes.Add(new Mesh());

        return (spline);
    }

    public void DeleteSpline()
    {
        DestroyImmediate(nodes[nodes.Count - 1].gameObject);
        DestroyImmediate(splines[splines.Count - 1].gameObject);

        nodes.RemoveAt(nodes.Count - 1);
        splines.RemoveAt(splines.Count - 1);
        meshes.RemoveAt(meshes.Count - 1);
    }

    public void Extrude()
    {
        if (extrudeShape == null)
        {
            extrudeShape = new ExtrudeShape();
        }        

        for (int i = 0; i < splines.Count; ++i)
        {
            splines[i].Extrude(meshes[i], extrudeShape);
        }
    }    

    public void AddSpline ()
    {
        if (closed || connected) return;

        if (splines.Count == 0)
        {
            // Create the first segment           
            Node firstNode = CreateNode(transform.position, transform.rotation);
            Node secondNode = CreateNode(transform.position + transform.forward * newNodeDistance, transform.rotation);
            CreateSpline(firstNode, secondNode);
        }
        else
        {
            Node lastNode = nodes[nodes.Count - 1];          
            Node newNode = CreateNode(lastNode.position + lastNode.transform.forward * newNodeDistance, lastNode.transform.rotation);
            CreateSpline(lastNode, newNode);
        }
    }	

    public void CloseCurve()
    {
        if (closed || connected) return;

        Node lastNode = nodes[nodes.Count - 1];
        Node firstNode = nodes[0];
        CreateSpline(lastNode, firstNode);

        closed = true;       
    }

    public void ClearCurve()
    {
        // Clear node references
        for (int i = 0; i < nodes.Count; ++i)
        {
            if(nodes[i] != null)
                DestroyImmediate(nodes[i].gameObject);
        }
        nodes.Clear();

        // Clear spline references
        for (int i = 0; i < splines.Count; ++i)
        {
            if (splines[i] != null)
                DestroyImmediate(splines[i].gameObject);
        }
        splines.Clear();

        // Clear mesh references
        for (int i = 0; i < meshes.Count; ++i)
        {
            if (meshes[i] != null)
                DestroyImmediate(meshes[i]);
        }
        meshes.Clear();

        // destroy other unreferenced elements        
        while(transform.childCount != 0)
        {
            if(transform.GetChild(0) != null)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        closed = false;

    }

        
}
