using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        Debug.Log("Executing Menu State");
        string level = gm.GetLevel();

        // Loads car selection after number of human players is selected.
        if (gm.numberOfHumanPlayers != 0 && level == "_Scenes/GameScenes/MENU_PLAYER")
        {
            gm.SetLevel("MENU_CAR");
        }
        else if (level == "_Scenes/GameScenes/MENU_CAR")
        {
            // This causes infinite loops!
            for (int i = 0; i < gm.numberOfHumanPlayers; i++)
            {
                if (gm.carSelected[i] && i < gm.numberOfHumanPlayers - 1)
                {
                    GameObject.Find("PlayerText").GetComponent<Text>().text = gm.nameOfPlayers[i + 1];
                }
                else
                {
                    i--;
                }
            }
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
