using UnityEngine;
using System.Collections;

public class RaceSetupState : State<GameManager>
{
    StateMachine<GameManager> fsm;

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        //Debug.Log("Executing RaceSetupState");
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

                GameObject playerHUD = (GameObject)GameObject.Instantiate(gm.playerHUDPrefab);
                playerHUD.GetComponent<PlayerHUD>().carController = gm.cars[i].GetComponent<CarController>();
                playerHUD.GetComponent<PlayerHUD>().Initialize();
                playerHUD.SetActive(true);

            }
            gm.carsInstantiated = true;
        }

        // Begin count down...
        // Switch to RaceState.
        GameObject.FindObjectOfType<Countdown>().countdownFinishedEvent += OnCountdownFinished;
    }

    public void enter(GameManager gm)
    {
        //Debug.Log("Entering RaceSetupState");
        SplitScreenCamera.totalPlayers = gm.numberOfHumanPlayers;
        fsm = gm.fsm;
        // Spawn locations not detected in the scene at this point... for some reason.
        // Placed spawn location setup and car initiation in execute insetad.
    }

    public void exit(GameManager gm)
    {
        //Debug.Log("Exiting RaceSetupState");
        GameObject.FindObjectOfType<Countdown>().countdownFinishedEvent -= OnCountdownFinished;

    }

    public void OnCountdownFinished()
    {
        if(fsm.getState().GetType() != typeof(RaceState))
            fsm.SetState(new RaceState());
    }
}
