using UnityEngine;
using System.Collections;

public class GameManagerEventCalls : MonoBehaviour {

    GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        gm.numberOfHumanPlayers = numberOfPlayers;
    }
}
