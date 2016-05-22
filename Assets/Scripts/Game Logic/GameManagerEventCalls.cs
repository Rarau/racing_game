using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerEventCalls : MonoBehaviour {

    GameManager gm;

	// Use this for initialization.
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    // Sets the number of players in the GameManager.
    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        gm.numberOfHumanPlayers = numberOfPlayers;
        gm.InitialisePlayerInformation();
    }

    //
    public void SetNumberOfLaps(int laps)
    {
        gm.numberOfLaps = laps;
    }

    // Sets the players car in the GameManager.
    public void SetPlayerCar(int car) 
    {
        int player = 0;
        string playerText = GameObject.Find("PlayerText").GetComponent<Text>().text;

        // Determine if the player is in double digits.
        if (playerText.Length == 7)
        {
            player = (int)char.GetNumericValue(playerText[playerText.Length - 1]);
        }
        else
        {
            playerText = "" + playerText[playerText.Length - 2] + playerText[playerText.Length - 1];
            player = int.Parse(playerText) - 1;
        }

        gm.playerCars[player] = car;
        gm.carSelected[player] = true;
        
        // Update player menu text.
        if (gm.numberOfHumanPlayers > 1)
        {
            if (player == 0)
            {
                GameObject.Find("PlayerText").GetComponent<Text>().text = "Player " + (player + 2);
            } 
            else if (player <= gm.numberOfHumanPlayers - 2)
            {
                GameObject.Find("PlayerText").GetComponent<Text>().text = "Player " + (player + 2);
            }
        }
    }
}
