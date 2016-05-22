using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    float speed = 100;

    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
