using UnityEngine;
using System.Collections;

public class StartState : State<GameManager> 
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Start State");

    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Start State");
        // Play intro animation
        // Set up player cars or initialization
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Start State");
    }
}


public class CountDownState : State<GameManager>
{
    GameManager gm;

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Start State");

    }

    public void enter(GameManager gm)
    {
        this.gm = gm;
        Debug.Log("Entered Start State");
        // Play intro animation
        // Set up player cars or initialization
    }

    public void exit(GameManager gm)
    {
    }

    public void OnCountdownFinished()
    {
        gm.fsm.setState(new RaceState());
    }
}

public class RaceState : State<GameManager>
{
    GameManager gm;

    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {

    }

    public void enter(GameManager gm)
    {
        this.gm = gm;

    }

    public void exit(GameManager gm)
    {
    }


}
