using UnityEngine;
using System.Collections.Generic;

public class RaceInfo : MonoBehaviour
{
    public GameManager gm;
    public CarController carController;

    public int lap = 1;
    public int racePosition = 0;
    public int lastCheckpoint = 0;

    public float lapTimer = 0.0f;
    public float cumulativeLapTimes = 0.0f;
    public List<float> lapTimes;

    public int gear = 0;
    public float speed = 0.0f;
    public Vector3 position;
    public Quaternion rotation;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        carController = GetComponent<CarController>();
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

        // Record car data for flips and pauses.
        //if (!IsFlipped())
        //{
        //    gear = carController.currentGear;
        //    speed = carController.currentSpeed;
        //    position = transform.position;
        //    rotation = transform.rotation;
        //}
        //else
        //{
        //    // Reposition car.
        //    carController.currentGear = 0;
        //    carController.currentSpeed = 0.0f;
        //    transform.position = position;
        //    transform.rotation = rotation;
        //    Debug.Log("Repositioned Car");
        //}
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

    // Check to see if the car is stuck.
    private bool IsFlipped()
    {
        if (carController.IsFlying() && carController.currentSpeed > 1.0f)
        {
            return true;
        }
        return false;
    }
}