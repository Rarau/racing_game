using UnityEngine;
using System.Collections;

public class SplashState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Splash State");

        // Load the player menu when start or space is pressed.
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Space))
        {
            gm.SetLevel("MENU_PLAYER");
            fsm.SetState(new MenuState());
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