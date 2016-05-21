using UnityEngine;
using System.Collections;

public class SplashState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Splash State");

        if (Input.anyKeyDown)
        {
            gm.SetLevel("MENU_PLAYER");
            fsm.setState(new MenuState());
        }

    }

    public void enter(GameManager gm)
    {
        Debug.Log("Entered Splash State");
    }

    public void exit(GameManager gm)
    {
        Debug.Log("Left Splash State");
    }
}