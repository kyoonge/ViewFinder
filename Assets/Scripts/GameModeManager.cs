using UnityEngine.Events;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameModeType CurrentMode { get; private set; }
    public UnityEvent<GameModeType> OnGameModeChanged = new UnityEvent<GameModeType>();
    public UnityEvent OnTakeAPicture = new UnityEvent();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CurrentMode = GameModeType.Normal;
    }

    public void ChangeState(GameModeType newMode)
    {
        CurrentMode = newMode;
        OnGameModeChanged.Invoke(CurrentMode);
    }

    public void TakingAPicture()
    {
        OnTakeAPicture.Invoke();
    }
}

public enum GameModeType
{
    Normal,
    Camera,
    Film
}