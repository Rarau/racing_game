using UnityEngine;
using System.Collections;

public class RaceSetupState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing RaceSetupState");
        // Setup spawn locations.
        if (!gm.spawnsSet)
        {
            gm.spawnLocations = new GameObject[gm.numberOfHumanPlayers + gm.numberOfAIPlayers];
            for (int i = 0; i < gm.spawnLocations.Length; i++)
            {
                gm.spawnLocations[i] = GameObject.Find("Position" + i);
            }
            gm.spawnsSet = true;
        }

        // Instantiate player cars.
        if (!gm.carsInstantiated)
        {
            for (int i = 0; i < gm.numberOfHumanPlayers; i++)
            {
                // Must make sure input / car movement is disabled.
                gm.cars.Add((GameObject)GameObject.Instantiate(gm.carsPrefab[gm.playerCars[i]], gm.spawnLocations[i].transform.position, gm.spawnLocations[i].transform.rotation));
                gm.cars[i].GetComponent<CarController>().playerNumber = i + 1;
                gm.cars[i].GetComponentInChildren<SplitScreenCamera>().SetPlayerNumber(i + 1);
                gm.cars[i].GetComponent<CarPlayerInput>().enabled = true;
                gm.cars[i].GetComponent<CarController>().SetEnableMotion(false);

            }
            gm.carsInstantiated = true;
        }

        // Begin count down...
        // Switch to RaceState.
        fsm.SetState(new RaceState());
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entering RaceSetupState");
        SplitScreenCamera.totalPlayers = gm.numberOfHumanPlayers;

        // Spawn locations not detected in the scene at this point... for some reason.
        // Placed spawn location setup and car initiation in execute insetad.
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Exiting RaceSetupState");
    }
}
