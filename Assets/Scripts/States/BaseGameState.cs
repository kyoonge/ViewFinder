public abstract class BaseGameState : IGameState
{
    protected GameModeManager gameManager;
    protected PlayerController player;

    public BaseGameState(GameModeManager manager, PlayerController playerController)
    {
        gameManager = manager;
        player = playerController;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }

    public virtual void HandleAction() { }
    public virtual void Update() { }
}