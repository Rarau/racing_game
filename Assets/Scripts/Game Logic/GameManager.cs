using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    //public enum LevelState { SPLASH_SCREEN, MAIN_MENU, CONFIG, RACE, FINISH_GRID };
    //public enum GameState { START, COUNTDOWN, RACE, PAUSE, END };
    //public GameState currentState;

    public StateMachine<GameManager> fsm;

    public int numberOfHumanPlayers;
    public int numberOfAIPlayers = 8;
    public int numberOfLaps;
    public bool spawnsSet;
    public bool carsInstantiated;
    public GameObject[] carsPrefab;
    public GameObject[] spawnLocations;
    public List<string> nameOfPlayers;
    public List<int> playerCars;
    public List<bool> carSelected;

    //public string[] nameOfPlayers;
    //public int trackSelected;
    //public bool[] carSelected;

    //const int numPlayers = 4;

    // To hold player data when paused.
    //struct PlayerData
    //{
    //    float speed;
    //    Vector3 position;
    //    int gear;
    //}

    //List<PlayerData> playerData = new List<PlayerData>();

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
        fsm.SetState(new SplashState());

        numberOfHumanPlayers = 0;
        numberOfLaps = 0;
        spawnsSet = false;
        carsInstantiated = false;
        nameOfPlayers = new List<string>();
        playerCars = new List<int>();
        carSelected = new List<bool>();
    }

    public void Update()
    {
        fsm.update();
    }

    public void InitialisePlayerInformation()
    {
        for (int i = 0; i < numberOfHumanPlayers; i++)
        {
            nameOfPlayers.Add(string.Concat("Player" + i.ToString()));
            playerCars.Add(0);
            carSelected.Add(false);
        }
    }


    public string GetLevel()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void SetLevel(string levelName)
    {
        SceneManager.LoadScene("_Scenes/GameScenes/" + levelName);
    }

    void Awake()
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