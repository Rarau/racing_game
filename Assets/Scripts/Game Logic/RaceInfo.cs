using UnityEngine;
using System.Collections.Generic;

public class RaceInfo : MonoBehaviour
{
    public GameManager gm;

    public int lap = 1;
    public int racePosition = 0;
    public int lastCheckpoint = 0;

    public float lapTimer = 0.0f;
    public float cumulativeLapTimes = 0.0f;
    public List<float> lapTimes;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        lapTimes = new List<float>();
    }

    void Update()
    {
        // After the first lap subtract the race timer to get the car's lap time.
        if (lap != 1)
        {
            lapTimer = Time.time - cumulativeLapTimes;
        }
        else
        {
            lapTimer = Time.time;
        }
    }

    // Stores the latest lap time and refreshes the lap timer.
    public void SaveLapTime()
    {
        lapTimes.Add(lapTimer);
        cumulativeLapTimes += lapTimer;
        lapTimer = 0.0f;
    }

    // Set the car's last checkpoint.
    public void SetLastCheckpoint(int checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    // Returns the car's last checkpoint that it travelled through.
    public int GetLastCheckpoint()
    {
        return lastCheckpoint;
    }

    // Set the value car's lap value.
    public void SetLap(int currentLap)
    {
        lap = currentLap;
    }

    // Returns the car's current lap value.
    public int GetLap()
    {
        return lap;
    }

    // Set the current lap time.
    public void SetLapTimer(float lapTime)
    {
        lapTimer = lapTime;
    }
}