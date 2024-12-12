using static GameModeManager;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse Sensitivity")]
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityX = 8f;  // 좌우 감도
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityY = 8f;  // 상하 감도

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private GameObject filmPrefab;
    [SerializeField] private GameObject cameraUI;

    private Vector2 moveInput;
    private float xRotation = 0f;

    private void Start()
    {
        SetupMouseCursor();
        SubscribeToGameModeChanges();
    }

    private void SetupMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SubscribeToGameModeChanges()
    {
        GameModeManager.Instance.OnGameModeChanged.AddListener(HandleGameModeChanged);
    }

    private void HandleGameModeChanged(GameModeType newMode)
    {
        cameraUI.SetActive(newMode == GameModeType.Camera);
    }

    public void SetMovementInput(Vector2 input)
    {
        moveInput = input;
    }

    public void HandleLookInput(float mouseX, float mouseY)
    {
        // X축과 Y축 감도를 개별적으로 적용
        mouseX *= mouseSensitivityX;
        mouseY *= mouseSensitivityY;

        // deltaTime 적용 - 프레임과 무관하게 일정한 회전 속도 유지
        mouseX *= Time.deltaTime * 10f;  // 전체적인 회전 느낌을 좀 더 부드럽게 하기 위해 10을 곱합니다
        mouseY *= Time.deltaTime * 10f;

        // 상하 회전 각도 계산 및 제한
        xRotation -= mouseY;  // 마우스 상하 반전이 필요없다면 += 로 변경
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 카메라 홀더의 상하 회전 적용
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 플레이어 좌우 회전
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    public void PlaceFilm()
    {
        Vector3 position = cameraHolder.position + cameraHolder.forward * 2f;
        Quaternion rotation = Quaternion.LookRotation(-cameraHolder.forward);
        Instantiate(filmPrefab, position, rotation);
    }
}