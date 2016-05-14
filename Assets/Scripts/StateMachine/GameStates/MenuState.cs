using UnityEngine;
using System.Collections;

public class MenuState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Menu State");
        if (Input.anyKey)
        {
            gm.setLevel("RACE");
            fsm.setState(new RaceStartState());
        }
    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Menu State");
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Menu State");
    }
}
