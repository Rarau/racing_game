using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    //public enum LevelState { SPLASH_SCREEN, MAIN_MENU, CONFIG, RACE, FINISH_GRID };
    //public enum GameState { START, COUNTDOWN, RACE, PAUSE, END };
    //public GameState currentState;

    public StateMachine<GameManager> fsm;

    //public int numberOfHumanPlayers;
    //public string[] nameOfPlayers;
    //public int trackSelected;
    //public bool[] carSelected;

    //const int numPlayers = 4;

    // To hold player data when paused.
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
            // Create logic to create the instance.
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
        // Setup state machine.
        fsm = new StateMachine<GameManager>(this);
        fsm.setState(new SplashState());

        //numberOfHumanPlayers = 0;
        //nameOfPlayers = new string[numPlayers];
        //carSelected = new bool[numPlayers];
        //for (int i = 0; i < numPlayers; i++)
        //{
        //    nameOfPlayers[i] = string.Concat("Player 0", i.ToString());
        //    carSelected[i] = false;
        //}

        //trackSelected = 0;
    }

    public void Update()
    {
        fsm.update();
    }

    string getLevel()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void setLevel(string levelName)
    {
        SceneManager.LoadScene("_Scenes/GameScenes/" + levelName);
    }

    void Awake ()
    {
        _instance = this;
        DontDestroyOnLoad(_instance);
	}

    // Sets the instance to null when the application quits
    public void OnApplicationQuit()
    {
        _instance = null;
    }
}