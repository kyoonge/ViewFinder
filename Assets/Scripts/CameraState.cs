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
        // ������̵� ī�޶� UI Ȱ��ȭ
        polaroid.transform.DOMove(polaroid.CameraModePos, 0.5f);
    }

    public override void HandleInput(PlayerController player)
    {
        // ���� �Կ� ���� �Է� ó��
    }
}