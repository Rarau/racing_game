using UnityEngine;
using System.Collections;

public class RaceStartState : State<GameManager> 
{

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Race Start State");
        fsm.setState(new RaceState());
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Race Start State");
        // Play intro animation
        // Set up player cars or initialization
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Race Start State");
    }
}
