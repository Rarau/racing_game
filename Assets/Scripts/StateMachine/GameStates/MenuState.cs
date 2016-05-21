using UnityEngine;
using System.Collections;

public class MenuState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Menu State");
        if (gm.numberOfHumanPlayers != 0 && gm.GetLevel() == "_Scenes/GameScenes/PLAYER_MENU")
        {
            gm.SetLevel("CAR_MENU");
        }

        //// Load the race with default settings when any key is pressed.
        //if (Input.anyKey)
        //{
        //    gm.SetLevel("RACE");
        //    fsm.setState(new RaceStartState());
        //}
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
