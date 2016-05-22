using UnityEngine;
using System.Collections;

public class ScoreboardState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Scoreboard State");
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Scoreboard State");
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Exiting Scoreboard State");
    }
}

