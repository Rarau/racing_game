using UnityEngine;
using System.Collections.Generic;

public class RaceInfo : MonoBehaviour
{
    public GameManager gm;

    public int lap = 0;
    public int racePosition = 0;
    public int lastCheckpoint = 0;

    public float lapTime = 0;
    public float timeSinceLastCheckpoint = 0;
    public List<float> lapTimes;

    void Start()
    {
        lapTimes = new List<float>();
    }

    public void addLapTime()
    {

    }

    //public void SetLastCheckpoint(int checkpoint)
    //{
    //    lastCheckpoint = checkpoint;
    //}

    //public int GetLastCheckpoint()
    //{
    //    return lastCheckpoint;
    //}

    //public void SetLap(int currentLap)
    //{
    //    lap = currentLap;
    //}

    //public int GetLap()
    //{
    //    return lap;
    //}
}