using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Terrain))]
public class TerrainShaper : MonoBehaviour {
    public GameObject trackRoot;

    Terrain terrain;
    TerrainData tData;

    public int widht;
    public int height;
    float[,] heights;


	// Use this for initialization
	void Start () 
    {
        terrain = GetComponent<Terrain>();
        tData = terrain.terrainData;
        height = tData.heightmapHeight;
        widht = tData.heightmapWidth;

        heights = tData.GetHeights(0, 0, widht, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < widht; x++)
            {
                heights[x, y] = 1.0f;
            }
        }

        //tData.SetHeights(0, 0, heights);
    }
	

    void ProcessTrackSegment(GameObject trackSegment)
    {
        Mesh mesh = trackSegment.GetComponent<Mesh>();

        foreach (Vector3 v in mesh.vertices)
        {
            Vector3 vLocal = transform.InverseTransformPoint(trackSegment.transform.TransformPoint(v));
            
        }

    }


	// Update is called once per frame
	void Update () {
	
	}
}
