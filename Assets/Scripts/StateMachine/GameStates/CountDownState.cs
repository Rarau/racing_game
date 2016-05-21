using UnityEngine;
using System.Collections;

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
    }

    public void exit(GameManager gm)
    {
    }

    public void OnCountdownFinished()
    {
        gm.fsm.SetState(new RaceState());
    }
}
