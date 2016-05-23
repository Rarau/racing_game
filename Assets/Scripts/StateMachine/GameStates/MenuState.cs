using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuState : State<GameManager>
{
    public void execute(GameManager gm, StateMachine<GameManager> fsm)
    {
        //Debug.Log("Executing Menu State");
        string level = gm.GetLevel();

        // Loads car selection after number of human players is selected.
        if (gm.numberOfHumanPlayers != 0 && level == "_Scenes/GameScenes/MENU_PLAYER")
        {
            gm.SetLevel("MENU_CAR");
        }
        else if (level == "_Scenes/GameScenes/MENU_CAR")
        {
            if (AllCarsSelected(gm.carSelected))
            {
                gm.SetLevel("MENU_LAPS");
            }           
        }
        else if (level == "_Scenes/GameScenes/MENU_LAPS")
        {
            if (gm.numberOfLaps != 0)
            {
                gm.SetLevel("RACE");
                fsm.SetState(new RaceSetupState());
            }
        }
    }

    public void enter(GameManager gm)
    {
        //Debug.Log("Entered Menu State");
    }

    public void exit(GameManager gm)
    {
        //Debug.Log("Left Menu State");
    }

    public bool AllCarsSelected(List<bool> carsSelected) 
    {
        for (int i = 0; i < carsSelected.Count; i++)
        {
            if (!carsSelected[i])
            {
                return false;
            }
        }
        return true;
    }
}
