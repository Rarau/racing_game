using UnityEngine;
using System.Collections.Generic;

public class RaceState : State<GameManager>
{
    // Temporary until we figure out where this is coming from.
    public List<GameObject> tempCarArray;

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        //Debug.Log("Executing Race State");
        CalculateRacePositions();
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Race State");
        // Get car starting position array.
        // Temp stuff until we figure out where the list of cars is coming from.
        tempCarArray = new List<GameObject>();
        tempCarArray.Add(GameObject.Find("CarSupra"));
        tempCarArray.Add(GameObject.Find("CarSupra1"));
        tempCarArray.Add(GameObject.Find("CarSupra2"));
        tempCarArray.Add(GameObject.Find("CarSupra3"));
        tempCarArray.Add(GameObject.Find("CarSupra4"));
        for (int i = 0; i < tempCarArray.Count; i++)
        {
            Debug.Log(tempCarArray[i].name);
        }
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Race State");
    }

    private void CalculateRacePositions()
    {
        tempCarArray.Sort(CompareByLap);
        for (int i = 0; i < tempCarArray.Count; i++)
        {
            Debug.Log(tempCarArray[i].name + ": " + tempCarArray[i].GetComponent<RaceInfo>().lap);
        }
        // Order car list based on lap number - the highest being index 0.
        // If multiple cars on the same lap take that subset and order based on last checkpoint.
        // If multiple last checkpoints take that subset and order based on distance to next checkpoint.
    }

    // Comparitor for the car's current lap.
    private int CompareByLap(GameObject car1, GameObject car2)
    {
        return car1.GetComponent<RaceInfo>().lap.CompareTo(car2.GetComponent<RaceInfo>().lap);
    }
}
