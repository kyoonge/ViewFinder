using UnityEngine;

public interface IGameState
{
    void EnterState();
    void ExitState();
    void HandleInput(PlayerController player);
    void Update();
}