using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Game Settings")]
    [SerializeField] private int scoreToWin = 5;
    [SerializeField] private GameState gameState = GameState.RoundSetup;

    private static GameManager _instance;

    private int playerScore;
    private int aiScore;

    private int currentRound;
    private GameState lastState;

    public StateGameStart stateGameStart { get; private set; }
    public StateRoundSetup stateRoundSetup { get; private set; }
    public StateRoundPlaying stateRoundPlaying { get; private set; }
    public StateRoundFinished stateRoundFinished { get; private set; }
    public StateGameFinished stateGameFinished { get; private set; }
    public StateGamePaused stateGamePaused { get; private set; }

    #endregion

    #region DELEGATES

    //Game State Delegates
    public delegate void OnGameStart();
    public static OnGameStart onGameStart;

    public delegate void OnRoundSetup();
    public static OnRoundSetup onRoundSetup;

    public delegate void OnRoundPlaying();
    public static OnRoundPlaying onRoundPlaying;

    public delegate void OnRoundFinished();
    public static OnRoundFinished onRoundFinished;

    public delegate void OnGamePaused();
    public static OnGamePaused onGamePaused;

    public delegate void OnGameFinished();
    public static OnGameFinished onGameFinished;

    public delegate void OnGameStateChanged();
    public static OnGameStateChanged onGameStateChanged;

    #endregion

    #region SINGLETON STUFF

    public static GameManager Instance 
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    _instance = singleton.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        stateGameStart = new StateGameStart();
        stateRoundSetup = new StateRoundSetup();
        stateRoundPlaying = new StateRoundPlaying();
        stateRoundFinished = new StateRoundFinished();
        stateGameFinished = new StateGameFinished();
        stateGamePaused = new StateGamePaused();
    }

    void Start()
    {
        currentRound = 1;
    }

    private void Update()
    {
        if (gameState != lastState)
        {
            onGameStateChanged?.Invoke();
        }

        switch (gameState)
        {
            case GameState.GameStart:
                stateGameStart.HandleState(ref gameState);
                break;
            case GameState.RoundSetup:
                stateRoundSetup.HandleState(ref gameState);
                break;
            case GameState.RoundPlaying:
                stateRoundPlaying.HandleState(ref gameState);
                break;
            case GameState.RoundFinished:
                stateRoundFinished.HandleState(ref gameState);
                break;
            case GameState.GameFinished:
                stateGameFinished.HandleState(ref gameState);
                break;
            case GameState.GamePaused:
                stateGamePaused.HandleState(ref gameState);
                break;
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    #endregion
}

public enum GameState
{
    GameStart,
    RoundSetup,
    RoundPlaying,
    GamePaused,
    RoundFinished,
    GameFinished
}
