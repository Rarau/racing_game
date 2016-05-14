using UnityEngine;
using System.Collections;

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
