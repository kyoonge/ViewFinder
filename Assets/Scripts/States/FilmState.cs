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
              // �ʸ� ����� �ֿ� �׼�: �ʸ� ��ġ
              //Vector3 position = player.GetFilmPlacementPosition();
              //polaroid.PlaceFilm(position);
              Debug.Log("�ʸ���ġ");
              gameManager.ChangeState(GameModeType.Normal); // �ʸ� ��ġ �� �Ϲ� ���� ��ȯ
       }

       public override void ExitState()
       {
              polaroid.HideFilm();
              player.SetFilmMode(false);
       }
}