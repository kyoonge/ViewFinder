using UnityEngine;
using UnityEngine.Events;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public enum GameMode
    {
        Play,
        Camera
    }

    public GameMode CurrentMode { get; private set; }
    public UnityEvent<GameMode> OnGameModeChanged = new UnityEvent<GameMode>();
    public UnityEvent OnTakeAPicture = new UnityEvent();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentMode = GameMode.Play;
    }

    public void ToggleMode()
    {
        CurrentMode = CurrentMode == GameMode.Play ? GameMode.Camera : GameMode.Play;
        OnGameModeChanged.Invoke(CurrentMode);
    }

    public void TakingAPicture()
    {
        OnTakeAPicture.Invoke();
    }

    public bool IsPlayMode() => CurrentMode == GameMode.Play;
    public bool IsCameraMode() => CurrentMode == GameMode.Camera;
}