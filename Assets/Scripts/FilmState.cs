using UnityEngine;

public class FilmState : BaseGameState
{
    public FilmState(GameModeManager manager, PlayerController playerController)
        : base(manager, playerController) { }

    public override void EnterState()
    {
        Debug.Log("Entering Film Mode");
        // 필름 배치 UI 활성화
    }

    public override void HandleInput(PlayerController player)
    {
        // 필름 배치 관련 입력 처리
    }
}