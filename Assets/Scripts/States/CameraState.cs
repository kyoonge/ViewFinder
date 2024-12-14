using UnityEngine;

public class CameraState : BaseGameState
{
       private readonly Polaroid polaroid;

       public CameraState(GameModeManager manager, PlayerController playerController, Polaroid polaroidRef)
           : base(manager, playerController)
       {
              polaroid = polaroidRef ?? throw new System.ArgumentNullException(nameof(polaroidRef));
       }

       public override void EnterState()
       {
              Debug.Log("Entering Camera Mode");
              polaroid.ActivateCamera(true);
              player.SetCameraMode(true);
              polaroid.HideFilm();
       }

       public override void HandleAction()
       {
              // ī�޶� ����� �ֿ� �׼�: ���� ���
              polaroid.TakePicture();
              gameManager.ChangeState(GameModeType.Film); // ������ ������ �ڵ����� �ʸ� ���� ��ȯ
       }

       public override void ExitState()
       {
              polaroid.ActivateCamera(false);
              player.SetCameraMode(false);
       }
}