using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerEventCalls : MonoBehaviour {

    GameManager gm;

	// Use this for initialization
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
    public void SetPlayerCar(string car) 
    {
        int player = 0;
        string playerText = GameObject.Find("PlayerText").GetComponent<Text>().text;
        if (playerText.Length == 7)
        {
            player = (int)char.GetNumericValue(playerText[playerText.Length - 1]);
        }
        else
        {
            playerText = "" + playerText[playerText.Length - 2] + playerText[playerText.Length - 1];
            player = int.Parse(playerText);
        }

        gm.playerCars[player] = car;
        gm.carSelected[player] = true;
    }
}
