using UnityEngine;
using System.Collections;

public class RaceSetupState : State<GameManager> 
{

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Race Start State");
        // Start countdown.
        fsm.SetState(new RaceState());
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Race Start State");
        // Play intro animation.
        // Initialise player vehicles and position them on track.
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Race Start State");
    }
}
