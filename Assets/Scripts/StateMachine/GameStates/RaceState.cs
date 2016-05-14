using UnityEngine;
using System.Collections;

public class RaceState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Race State");
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Race State");
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Race State");
    }
}
