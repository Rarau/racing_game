using UnityEngine;
using System.Collections.Generic;

public class RaceState : State<GameManager>
{
    public List<GameObject> cars;

    private GameManager gm;

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        //Debug.Log("Executing Race State");
        UpdateRacePositions();

        // Check if all cars have crossed the finish line and end race.
        if (AllCarsFinished())
        {
            // Load scoreboard.
            GameObject scoreboard = GameObject.FindGameObjectWithTag("Scoreboard");
            scoreboard.GetComponent<Canvas>().enabled = true;
        };
    }

    public void enter(GameManager gm)
    {
        //Debug.Log("Entered Race State");
        this.gm = gm;

        // Get the list of cars from the game manager to manipulate.
        cars = gm.cars;

        // Remove startup time from the race timer.
        float raceStartTime = Time.time;
        
        // Adjust the cars lap timers to account the the start time.
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].GetComponent<RaceInfo>().SetLapTimer(Time.time - raceStartTime);
            gm.cars[i].GetComponent<CarController>().SetEnableMotion(true);
        }
    }

    public void exit(GameManager gm)
    {
        //Debug.Log("Left Race State");
    }

    private void UpdateRacePositions()
    {
        List<GameObject> cars = CalculateRacePositions();
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].GetComponent<RaceInfo>().racePosition = i + 1;
        }
    }

    // Determine the position of each car in the race.
    private List<GameObject> CalculateRacePositions()
    {
        // Order car list based on lap number - zero index = last place.
        cars.Sort(CompareByLap);

        // For each car on a lap find all other cars on the same lap.
        List<int> sortedLaps = new List<int>();
        for (int i = 0; i < cars.Count; i++)
        {
            int lap = cars[i].GetComponent<RaceInfo>().lap;
            List<GameObject> sameLapCars;

            if (!sortedLaps.Contains(lap))
            {
                sameLapCars = cars.FindAll(delegate (GameObject car) { return car.GetComponent<RaceInfo>().lap == lap; });

                if (sameLapCars.Count > 1)
                {
                    // Sort via last checkpoint.
                    sameLapCars.Sort(CompareByCheckpoint);

                    // Find any cars who have the same last checkpoint on the same lap.
                    List<int> sortedCheckpoints = new List<int>();
                    for (int j = 0; j < sameLapCars.Count; j++)
                    {
                        int lastCheckpoint = sameLapCars[j].GetComponent<RaceInfo>().lastCheckpoint;
                        List<GameObject> sameLastCheckpointCars;

                        if (!sortedCheckpoints.Contains(lastCheckpoint))
                        {
                            sameLastCheckpointCars = sameLapCars.FindAll(delegate (GameObject car) { return car.GetComponent<RaceInfo>().lastCheckpoint == lastCheckpoint; });
                            if (sameLastCheckpointCars.Count > 1)
                            {
                                sameLastCheckpointCars.Sort(CompareByDistanceToNextCheckpoint);

                                // Replace the unordered cars by their distance to next checkpoint ordering.
                                for (int k = 0; k < sameLastCheckpointCars.Count; k++)
                                {
                                    sameLapCars[j + k] = sameLastCheckpointCars[k];
                                }
                            }
                        }

                        sortedCheckpoints.Add(lastCheckpoint);
                    }
                }

                // Replace the unordered cars by their last checkpoint ordering.
                for (int j = 0; j < sameLapCars.Count; j++)
                {
                    cars[i + j] = sameLapCars[j];
                }

                sortedLaps.Add(lap);
            }
        }

        // Index 0 is now the first position.
        cars.Reverse();

        //// For debugging.
        //for (int j = 0; j < cars.Count; j++)
        //{
        //    Debug.Log(cars[j].name);
        //}
        //Debug.Log("BREAK");
        //Debug.Log("BREAK");
        //Debug.Log("BREAK");

        return cars;
    }

    // Returns true when all cars have finished the race.
    private bool AllCarsFinished()
    {
        if (gm.finalPositions.Count == cars.Count)
        {
            // All cars have finished.
            return true;
        }
        return false;
    }

    // Comparitor for the cars current lap.
    private int CompareByLap(GameObject car1, GameObject car2)
    {
        return car1.GetComponent<RaceInfo>().lap.CompareTo(car2.GetComponent<RaceInfo>().lap);
    }

    // Comparitor for the cars last checkpoint.
    private int CompareByCheckpoint(GameObject car1, GameObject car2)
    {
        return car1.GetComponent<RaceInfo>().lastCheckpoint.CompareTo(car2.GetComponent<RaceInfo>().lastCheckpoint);
    }

    // Comparitor for the cars distance to their next checkpoints.
    private int CompareByDistanceToNextCheckpoint(GameObject car1, GameObject car2)
    {
        float car1Distance = CalculateDistance(car1, GameObject.Find("Checkpoint" + (car1.GetComponent<RaceInfo>().lastCheckpoint + 1).ToString()));
        float car2Distance = CalculateDistance(car2, GameObject.Find("Checkpoint" + (car2.GetComponent<RaceInfo>().lastCheckpoint + 1).ToString()));
        return car2Distance.CompareTo(car1Distance);
    }

    // Calculates the distance between two game objects.
    private float CalculateDistance(GameObject car, GameObject checkpoint)
    {
        return Vector3.Distance(car.transform.position, checkpoint.transform.position);
    }
}
