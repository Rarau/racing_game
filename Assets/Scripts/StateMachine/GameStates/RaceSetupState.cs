using UnityEngine;
using System.Collections;

public class RaceSetupState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
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
                GameObject.Instantiate(gm.carsPrefab[gm.playerCars[i]], gm.spawnLocations[i].transform.position, gm.spawnLocations[i].transform.rotation);
            }
            gm.carsInstantiated = true;
        }
    }

    public void enter(GameManager gm)
    {
        // Spawn locations not detected in the scene at this point... for some reason.
        // Placed spawn location setup and car initiation in execute insetad.
    }

    public void exit(GameManager gm)
    {

    }
}
