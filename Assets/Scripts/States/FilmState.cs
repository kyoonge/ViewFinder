using UnityEngine;

public class FilmState : BaseGameState
{
       private readonly Polaroid polaroid;

       public FilmState(GameModeManager manager, PlayerController playerController, Polaroid polaroidRef)
           : base(manager, playerController)
       {
              polaroid = polaroidRef ?? throw new System.ArgumentNullException(nameof(polaroidRef));
       }

       public override void EnterState()
       {
              Debug.Log("Entering Film Mode");
              polaroid.ShowFilm();
              player.SetFilmMode(true);
       }

       public override void HandleAction()
       {
              // 필름 모드의 주요 액션: 필름 배치
              //Vector3 position = player.GetFilmPlacementPosition();
              //polaroid.PlaceFilm(position);
              Debug.Log("필름배치");
              gameManager.ChangeState(GameModeType.Normal); // 필름 배치 후 일반 모드로 전환
       }

       public override void ExitState()
       {
              polaroid.HideFilm();
              player.SetFilmMode(false);
       }
}