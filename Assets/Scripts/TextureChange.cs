using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

    float maxHealth;

    Renderer renderer;

    MeshChange meshChange;

    //private MeshFilter meshFilter;

    public bool isParent = false;

    public Texture[] swapTexture;

    public bool isSupraBody = false;

    void Awake()
    {
        if (isParent)
            meshChange = transform.parent.GetComponent<MeshChange>();
        else
            meshChange = transform.parent.parent.GetComponent<MeshChange>();
    }

    // Use this for initialization
    void Start () {
        //meshFilter = this.GetComponent<MeshFilter>();
        renderer = this.GetComponent<Renderer>();
        maxHealth = 100;
    }
	
	// Update is called once per frame
	void Update () {
        if (meshChange.meshHealth > 0.0f)
        {
            if (meshChange.meshHealth >= maxHealth)
            {
                //renderer.material.mainTexture = Resources.Load("Bugatti_Alb") as Texture;
                //this.SetTexture("Texture", Resources.Load("Bugatti_Alb") as Texture); // how to load normal maps??
                if (isSupraBody)
                    renderer.materials[1].mainTexture = swapTexture[0];
                else
                    renderer.materials[0].mainTexture = swapTexture[0];
                //renderer.materials[index]
            }
            else if(meshChange.meshHealth <= 80)
            {
                //renderer.material.mainTexture = Resources.Load("Bugatti_D1_Alb") as Texture;
                if (isSupraBody)
                    renderer.materials[1].mainTexture = swapTexture[1];
                else
                    renderer.materials[0].mainTexture = swapTexture[1];
            }
        }
    }

}
