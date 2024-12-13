using Unity.VisualScripting;
using UnityEngine;

public class NormalState : BaseGameState
{
    Polaroid polaroid;
    public NormalState(GameModeManager manager, PlayerController playerController, Polaroid polaroidRef)
        : base(manager, playerController) 
    {
        polaroid = polaroidRef ?? throw new System.ArgumentNullException(nameof(polaroidRef));
    }

    public override void EnterState()
    {
        Debug.Log("Entering Normal Mode");
        polaroid.HideFilm();
        polaroid.ActivateCamera(false);
    }

    public override void ExitState()
    {
    }

}