using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SplitScreenCamera : MonoBehaviour 
{    
    // Default settings, can be changed from game manager
    public static int totalPlayers = 4;
    public static int numRows = 2;
    public static int numCols = 2;

    private int playerNum = 1;

    private Camera cam;

	// Use this for initialization
	void Start () 
    {
        cam = GetComponent<Camera>();

	}
	
    public void SetPlayerNumber(int num)
    {
        cam = GetComponent<Camera>();

        playerNum = num;

        if (totalPlayers == 1)
        {
            return;
        }
        else if(totalPlayers == 2)
        {
            float y = ((playerNum - 1) % numCols) / (float)numCols;
            //float y = ((int)((playerNum - 1) / (float)numRows)) / (float)numRows;
            y = -y + (1.0f / numRows);
            cam.rect = new Rect(new Vector2(0.0f, y), new Vector2(1.0f, 0.5f));
        }
        else if (totalPlayers == 3)
        {
            if(playerNum == 1 || playerNum == 2)
            {
                float x = ((playerNum - 1) % numCols) / (float)numCols;
                float y = ((int)((playerNum - 1) / (float)numRows)) / (float)numRows;
                y = -y + (1.0f / numRows);
                cam.rect = new Rect(new Vector2(x, y), new Vector2(1.0f / numCols, 1.0f / numRows));
            }
            else if(playerNum == 3)
            {
                float x = ((playerNum - 1) % numCols) / (float)numCols;
                float y = ((int)((playerNum - 1) / (float)numRows)) / (float)numRows;
                y = -y + (1.0f / numRows);
                cam.rect = new Rect(new Vector2(x, y), new Vector2(1.0f, 1.0f / numRows));
            }
        }
        else if (totalPlayers == 4)
        {
            float x = ((playerNum - 1) % numCols) / (float)numCols;
            float y = ((int)((playerNum - 1) / (float)numRows)) / (float)numRows;
            y = -y + (1.0f / numRows);
            cam.rect = new Rect(new Vector2(x, y), new Vector2(1.0f / numCols, 1.0f / numRows));
        }
    }

	// Update is called once per frame
	void Update () 
    {

	}
}
