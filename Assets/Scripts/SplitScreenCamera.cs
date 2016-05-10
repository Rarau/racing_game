using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SplitScreenCamera : MonoBehaviour 
{    
    // Default settings, can be changed from game manager
    public static int totalPlayers = 4;
    public static int numRows = 2;
    public static int numCols = 2;

    public int playerNum = 1;

    private Camera cam;

	// Use this for initialization
	void Start () 
    {
        cam = GetComponent<Camera>();

        float x = ((playerNum - 1) % numCols) / (float)numCols;
        float y = ((int)((playerNum - 1) / (float)numRows)) / (float)numRows;
        y = -y + (1.0f / numRows);
        cam.rect = new Rect(new Vector2(x, y), new Vector2(1.0f / numCols, 1.0f / numRows));
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
