using UnityEngine;

public class FilmState : BaseGameState
{
    public FilmState(GameModeManager manager, PlayerController playerController)
        : base(manager, playerController) { }

    public override void EnterState()
    {
        Debug.Log("Entering Film Mode");
        // �ʸ� ��ġ UI Ȱ��ȭ
    }

    public override void HandleInput(PlayerController player)
    {
        // �ʸ� ��ġ ���� �Է� ó��
    }
}