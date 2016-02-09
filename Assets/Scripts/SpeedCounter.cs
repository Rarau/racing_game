using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class SpeedCounter : MonoBehaviour
{
    public StraightLineTest car;
    public Rigidbody rb;

    private Text t;

    // Use this for initialization
    void Start()
    {
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t.text = (rb.velocity.magnitude * 3.6f).ToString("0.0 KMH") + "\n";
        t.text += "Engine rmp: " + car.rpm;
    }
}
