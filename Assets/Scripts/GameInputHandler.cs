using static GameModeManager;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    private PlayerController playerController;
    private Vector2 moveInput;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        playerController.HandleLookInput(mouseX, mouseY);
    }

    // 이동 입력 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        playerController.SetMovementInput(moveInput);
    }

    // 모드 변경 입력 처리
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

    public void OnFilmModeChange(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    GameModeManager.Instance.ChangeState(GameModeType.Film);
        //}
    }

    // 액션 입력 처리
    public void OnAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        switch (GameModeManager.Instance.CurrentMode)
        {
            case GameModeType.Camera:
                GameModeManager.Instance.TakingAPicture();
                break;
            case GameModeType.Film:
                playerController.PlaceFilm();
                break;
        }
    }
}