using UnityEngine;
using System.Collections;

public class RaceInfo : MonoBehaviour
{
    public GameManager gm;
    public int racePosition = 0;
    public int lap = 0;
    public int lastCheckpoint = 0;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //racePosition = gm.getPosition();
    }
}