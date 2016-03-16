using UnityEngine;
using System.Collections.Generic;

public abstract class TrackElement : MonoBehaviour
{
    public float trackWidth;
    public int horizontalDivisions;
    public int divisionsPerCurve = 5;

    public float newNodeDistance = 10;

    public List<Node> nodes;
    public List<BezierSpline> splines;

    public List<Mesh> meshes;

    public GameObject nodePrefab;
    public GameObject splinePrefab;

    public ExtrudeShape extrudeShape;

    public abstract BezierSpline CreateSpline(Node start, Node end);
    public abstract Node CreateNode(Vector3 position, Quaternion rotation);

}
