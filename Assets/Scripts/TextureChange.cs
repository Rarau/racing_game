using UnityEngine;
using System.Collections;

public class TextureChange : MonoBehaviour {

    float maxHealth;

    Renderer renderer;

    MeshChange meshChange;

    public bool isParent = false;

    void Awake()
    {
        if (isParent)
            meshChange = transform.parent.GetComponent<MeshChange>();
        else
            meshChange = transform.parent.parent.GetComponent<MeshChange>();
    }

    // Use this for initialization
    void Start () {
        renderer = this.GetComponent<Renderer>();
        maxHealth = 100;
    }
	
	// Update is called once per frame
	void Update () {
        //if (meshChange.meshHealth > 0.0f)
        {
            if (meshChange.meshHealth >= maxHealth)
            {
                renderer.material.mainTexture = Resources.Load("Bugatti_Alb") as Texture;
                //this.SetTexture("Texture", Resources.Load("Bugatti_Alb") as Texture); // how to load normal maps??
            }
            else
            {
                renderer.material.mainTexture = Resources.Load("Bugatti_D1_Alb") as Texture;
            }
        }
    }

}
