using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class NodeData
{
    public float gizmoSize;

    public SerializableVector3 frontControl;
    public SerializableVector3 backControl;

    public float trackWidthModifier;
    public float rightCurvature; // between 0 and 1
    public float leftCurvature; // between 0 and 1

    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public bool reverse;
}


[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    public bool reverse = false;

    public TrackElement curve;    

    public float gizmoSize = 0.2f;

    public float frontWeight = 1;
    public float backWeight = 1;

    public float trackWidthModifier = 0;
    public float rightCurvature = 0; // between 0 and 1
    public float leftCurvature = 0; // between 0 and 1

    public Transform frontTransform;
    public Transform backTransform;   

    public Vector3 frontControl
    {
        get {
            //return transform.position + transform.forward * frontWeight; 
            return frontTransform.position;
        }
        set { }
    }

    public Vector3 backControl
    {
        get {
            //return transform.position - transform.forward * backWeight;
            return backTransform.position;
        }
        set { }
    }

    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public NodeData Serialize()
    {
        NodeData data = new NodeData();
        data.gizmoSize = this.gizmoSize;

        data.frontControl = frontTransform.position;
        data.backControl = backTransform.position;

        data.trackWidthModifier = this.trackWidthModifier;
        data.rightCurvature = this.rightCurvature; // between 0 and 1
        data.leftCurvature = this.leftCurvature; // between 0 and 1

        data.position = transform.position;
        data.rotation = transform.rotation;

        data.reverse = reverse;

        return data;
    }

    public void Load(NodeData data)
    {        
        gizmoSize = data.gizmoSize;

        frontControl = data.frontControl;
        backControl = data.backControl;

        trackWidthModifier = data.trackWidthModifier;
        rightCurvature = data.rightCurvature; // between 0 and 1
        leftCurvature = data.leftCurvature; // between 0 and 1

        transform.position = data.position;
        transform.rotation = data.rotation;

        frontTransform.position = data.frontControl;
        backTransform.position = data.backControl;

        reverse = data.reverse;
    }

    public void Copy(Node other)
    {
        gizmoSize = other.gizmoSize;

        frontControl = other.frontControl;
        backControl = other.backControl;

        trackWidthModifier = other.trackWidthModifier;
        rightCurvature = other.rightCurvature; // between 0 and 1
        leftCurvature = other.leftCurvature; // between 0 and 1

        transform.position = other.position;
        transform.rotation = other.transform.rotation;                       
        
        frontTransform.position = other.frontControl;
        backTransform.position = other.backControl;

        if (reverse)
        {// facing opposite directions
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);

        }

    }

    void Awake()
    {
        frontTransform = transform.FindChild("front");
        backTransform = transform.FindChild("back");

    }

    void OnDrawGizmos()
    {
        frontTransform = transform.FindChild("front");
        backTransform = transform.FindChild("back");

        Gizmos.color = new Vector4(0, 0, 1, 1);
        Gizmos.DrawWireSphere(transform.position, gizmoSize);

        Gizmos.color = new Vector4(0.7f, 0.7f, 1, 1);
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * frontWeight);
        //Gizmos.DrawLine(transform.position, transform.position - transform.forward * backWeight);

        Gizmos.DrawLine(transform.position, frontTransform.position);
        Gizmos.DrawLine(transform.position, backTransform.position);

        //Gizmos.DrawWireSphere(transform.position + transform.forward * frontWeight, gizmoSize / 2);
        //Gizmos.DrawWireSphere(transform.position - transform.forward * backWeight, gizmoSize / 2);

        DebugExtension.DrawArrow(transform.position, transform.forward / 2, Color.cyan);

        // up and right indicators
        Gizmos.color = new Vector4(1, 0, 0, 1);
        Gizmos.DrawLine(transform.position, transform.position + transform.right*3);
        Gizmos.color = new Vector4(0, 1, 0, 1);
        Gizmos.DrawLine(transform.position, transform.position + transform.up*3);

    }    
}
