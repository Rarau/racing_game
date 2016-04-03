using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public enum LevelState { SPLASH_SCREEN, MAIN_MENU, CONFIG, RACE, FINISH_GRID };
    public enum GameState  { START, PAUSE, CANCEL, END };

    public int numberOfHumanPlayers;
    public string[] nameOfPlayers;
    public int trackSelected;
    public bool[] carSelected;

    const int numPlayers = 4;

    // to hold player data when pauses
    struct PlayerData
    {
        float speed;
        Vector3 position;
        int gear;
    }
    List<PlayerData> playerData = new List<PlayerData>();

    public static GameManager Instance
    {
        get
        {
            //create logic to create the instance
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
        //SceneManager.LoadScene(LevelState.SPLASH_SCREEN.ToString());
        numberOfHumanPlayers = 0;
        nameOfPlayers = new string[numPlayers];
        carSelected = new bool[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            nameOfPlayers[i] = string.Concat("Player 0", i.ToString());
            carSelected[i] = false;
        }

        trackSelected = 0;
    }

    string getLevel()
    {
        return SceneManager.GetActiveScene().name;
    }

    void setLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    void Awake ()
    {
        _instance = this;
	}

    // Sets the instance to null when the application quits
    public void OnApplicationQuit()
    {
        _instance = null;
    }
}