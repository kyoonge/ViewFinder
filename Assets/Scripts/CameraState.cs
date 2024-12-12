using DG.Tweening;
using UnityEngine;

public class CameraState : BaseGameState
{
    private Polaroid polaroid;

    public CameraState(GameModeManager manager, PlayerController playerController, Polaroid polaroidRef)
        : base(manager, playerController)
    {
        polaroid = polaroidRef;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Camera Mode");
        // 폴라로이드 카메라 UI 활성화
        polaroid.transform.DOMove(polaroid.CameraModePos, 0.5f);
    }

    public override void HandleInput(PlayerController player)
    {
        // 사진 촬영 관련 입력 처리
    }
}