using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Polaroid polaroid;

    public static GameModeManager Instance { get; private set; }
    private Dictionary<GameModeType, IGameState> states;
    private IGameState currentState;
    public GameModeType CurrentMode { get; private set; }

    public UnityEvent<GameModeType> OnGameModeChanged = new UnityEvent<GameModeType>();
    public IGameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeStates();
        SetInitialState();
    }

    private void InitializeStates()
    {
        states = new Dictionary<GameModeType, IGameState>
        {
            { GameModeType.Normal, new NormalState(this, playerController, polaroid) },
            { GameModeType.Camera, new CameraState(this, playerController, polaroid) },
            { GameModeType.Film, new FilmState(this, playerController, polaroid) }
        };
    }

    private void SetInitialState()
    {
        CurrentMode = GameModeType.Normal;
        currentState = states[CurrentMode];
        currentState.EnterState();
    }

    public void ChangeState(GameModeType newMode)
    {
        if (CurrentMode == newMode) return;
        if (!states.ContainsKey(newMode))
        {
            Debug.LogError($"State {newMode} not found!");
            return;
        }

        currentState?.ExitState();
        CurrentMode = newMode;
        currentState = states[newMode];
        currentState.EnterState();
        OnGameModeChanged.Invoke(CurrentMode);
    }

    private void Update()
    {
        currentState?.Update();
    }
}

public enum GameModeType
{
    Normal,
    Camera,
    Film
}