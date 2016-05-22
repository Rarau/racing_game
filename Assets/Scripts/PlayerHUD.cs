using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PlayerHUD : MonoBehaviour 
{
    public CarController carController;

    public Text lapCounter;
    public Text posCounter;

    public RaceInfo raceInfo;
    public int lastLap;
    public int lastPos;
    void Start()
    {
        Initialize();

    }
    
	// Use this for initialization
	public void Initialize () 
    {
        lastLap = 0;
        lastPos = 0;
        if (carController != null)
        {
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            GetComponent<Canvas>().planeDistance = 0.5f;
            GetComponent<Canvas>().worldCamera = carController.GetComponentInChildren<Camera>();
            GetComponentInChildren<RevCounter>().car = carController;
            GetComponentInChildren<MilesCounter>().car = carController;
            lapCounter = transform.FindChild("LapCounter/LapCount").GetComponent<Text>();
            posCounter = transform.FindChild("PosCounter/PosCount").GetComponent<Text>();

            raceInfo = carController.GetComponent<RaceInfo>();
            lapCounter.text = (raceInfo.lap - 1) + "/" + FindObjectOfType<GameManager>().numberOfLaps;
        }
	}

    public void Update()
    {
        if (raceInfo.racePosition != lastPos)
        {
            lastPos = raceInfo.racePosition;
            posCounter.text = (raceInfo.racePosition) + "/" + FindObjectOfType<GameManager>().numberOfHumanPlayers;
        }
        if (raceInfo.lap != lastLap)
        {
            lastLap = raceInfo.lap;
            lapCounter.text = (raceInfo.lap - 1) + "/" + FindObjectOfType<GameManager>().numberOfLaps;
        }
    }
	

}
