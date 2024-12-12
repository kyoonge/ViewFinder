using UnityEngine;

public class NormalState : BaseGameState
{
    public NormalState(GameModeManager manager, PlayerController playerController)
        : base(manager, playerController) { }

    public override void EnterState()
    {
        Debug.Log("Entering Normal Mode");
    }

    public override void HandleInput(PlayerController player)
    {
        // 일반 모드에서의 입력 처리
    }
}