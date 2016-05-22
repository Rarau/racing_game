using UnityEngine;
using System.Collections;

public class CountDownState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Start State");

    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Start State");
    }

    public void exit(GameManager gm)
    {
    }

    //public void OnCountdownFinished()
    //{
    //    fsm.SetState(new RaceState());
    //}
}
