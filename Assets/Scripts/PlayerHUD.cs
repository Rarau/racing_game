using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class PlayerHUD : MonoBehaviour 
{
    public CarController carController;


    void Start()
    {
        Initialize();
    }
    
	// Use this for initialization
	public void Initialize () 
    {
        if (carController != null)
        {
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            GetComponent<Canvas>().planeDistance = 0.5f;
            GetComponent<Canvas>().worldCamera = carController.GetComponentInChildren<Camera>();
            GetComponentInChildren<RevCounter>().car = carController;
            GetComponentInChildren<MilesCounter>().car = carController;
        }
	}
	
}
