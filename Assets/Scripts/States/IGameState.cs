using UnityEngine;

public interface IGameState
{
       void EnterState();
       void ExitState();
       void HandleAction();
       void Update();
}