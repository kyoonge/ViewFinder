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
              // 카메라 모드의 주요 액션: 사진 찍기
              polaroid.TakePicture();
              gameManager.ChangeState(GameModeType.Film); // 사진을 찍으면 자동으로 필름 모드로 전환
       }

       public override void ExitState()
       {
              polaroid.ActivateCamera(false);
              player.SetCameraMode(false);
       }
}