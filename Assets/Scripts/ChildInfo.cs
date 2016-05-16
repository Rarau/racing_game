using UnityEngine;
using System.Collections;

public class ChildInfo : MonoBehaviour {

    public Vector3 GetPosition () {
        Vector3 childPosition;
        childPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return childPosition;
    }
}
