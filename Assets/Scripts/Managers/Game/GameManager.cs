using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Game Settings")]
    [SerializeField] private int scoreToWin = 5;
    [SerializeField] private GameState gameState = GameState.RoundSetup;
    public Transform globalOrientation;

    private static GameManager _instance;

    public int playerScore { get; private set; }
    public int aiScore { get; private set; }

    public int currentRound;
    private GameState lastState;

    public StateGameStart stateGameStart { get; private set; }
    public StateRoundSetup stateRoundSetup { get; private set; }
    public StateRoundPlaying stateRoundPlaying { get; private set; }
    public StateRoundFinished stateRoundFinished { get; private set; }
    public StateGameFinished stateGameFinished { get; private set; }
    public StateGamePaused stateGamePaused { get; private set; }

    private IState currentStateClass;

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

    public delegate void OnScoreUpdated();
    public static OnScoreUpdated onScoreUpdated;

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

    #region ENABLE/DISABLE

    private void OnEnable()
    {
        ScoreZoneManager.onScore += OnPlayerScored;
    }

    private void OnDisable()
    {
        ScoreZoneManager.onScore -= OnPlayerScored;
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

        gameState = GameState.GamePaused;
        currentStateClass = stateGamePaused;
    }

    void Start()
    {
        currentRound = 0;
        ChangeState(GameState.GameStart);
    }

    private void Update()
    {
        currentStateClass.HandleState(ref gameState);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    #endregion

    #region METHODS

    private void OnPlayerScored(FlagType flag)
    {
        ChangeState(GameState.RoundFinished);

        if (flag == FlagType.None)
        {
            return;
        }
        else if (flag == FlagType.Blue)
        {
            playerScore++;
        }
        else if (flag == FlagType.Red)
        {
            aiScore++;
        }

        onScoreUpdated?.Invoke();

        if (playerScore == scoreToWin || aiScore == scoreToWin)
        {
            ChangeState(GameState.GameFinished);
            onGameFinished?.Invoke();
        }
    }

    public void ChangeState(GameState newState)
    {
        if (gameState != newState)//IF the state to change is NOT the same as the current game state
        {
            lastState = gameState;
            gameState = newState;
            currentStateClass.ExitState();

            switch (gameState)
            {
                case GameState.GameStart:
                    currentStateClass = stateGameStart;
                    break;
                case GameState.RoundSetup:
                    currentStateClass = stateRoundSetup;
                    break;
                case GameState.RoundPlaying:
                    currentStateClass = stateRoundPlaying;
                    break;
                case GameState.RoundFinished:
                    currentStateClass = stateRoundFinished;
                    break;
                case GameState.GameFinished:
                    currentStateClass = stateGameFinished;
                    break;
                case GameState.GamePaused:
                    currentStateClass = stateGamePaused;
                    break;
            }

            onGameStateChanged?.Invoke();
            currentStateClass.EnterState();
        }
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
