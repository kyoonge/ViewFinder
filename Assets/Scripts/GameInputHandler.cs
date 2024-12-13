using UnityEngine.InputSystem;
using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            playerController.HandleLookInput(mouseX, mouseY);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerController != null)
        {
            playerController.SetMovementInput(context.ReadValue<Vector2>());
        }
    }

    public void OnNormalModeChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameModeManager.Instance.ChangeState(GameModeType.Normal);
        }
    }

    public void OnCameraModeChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameModeManager.Instance.ChangeState(GameModeType.Camera);
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        GameModeManager.Instance.CurrentState.HandleAction();
    }
}