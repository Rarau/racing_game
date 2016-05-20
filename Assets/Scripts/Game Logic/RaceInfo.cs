using UnityEngine;
using System.Collections.Generic;

public class RaceInfo : MonoBehaviour
{
    public GameManager gm;

    public int lap = 0;
    public int racePosition = 0;
    public int lastCheckpoint = 0;

    public float lapTimer = 0.0f;
    public List<float> lapTimes;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        lapTimes = new List<float>();
    }

    void Update()
    {
        lapTimer = Time.time;
    }

    public void SetLastCheckpoint(int checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public int GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    public void SetLap(int currentLap)
    {
        lap = currentLap;
    }

    public int GetLap()
    {
        return lap;
    }
}