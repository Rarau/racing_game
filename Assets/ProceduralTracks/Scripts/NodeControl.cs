using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class NodeControl : MonoBehaviour
{
    public float gizmoSize = 0.2f;	
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.localPosition;
        pos.x = 0;
        pos.y = 0;
        transform.localPosition = pos;
    }

    void OnDrawGizmos()
    {        
        Gizmos.color = new Vector4(0, 1, 1, 1);
        Gizmos.DrawWireSphere(transform.position, gizmoSize);
    }
}
