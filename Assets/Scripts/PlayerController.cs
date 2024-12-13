using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float cameraModeSpeed = 3f;
    [SerializeField] private float filmModeSpeed = 4f;

    [Header("Mouse Sensitivity")]
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityX = 8f;
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityY = 8f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private GameObject filmPrefab;
    [SerializeField] private GameObject cameraUI;

    private Vector2 moveInput;
    private float xRotation = 0f;
    private float currentMoveSpeed;
    private bool isInCameraMode;
    private bool isInFilmMode;

    private void Start()
    {
        SetupMouseCursor();
        currentMoveSpeed = normalMoveSpeed;
    }

    private void SetupMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCameraMode(bool active)
    {
        isInCameraMode = active;
        currentMoveSpeed = active ? cameraModeSpeed : normalMoveSpeed;
        cameraUI.SetActive(active);
        
    }

    public void SetFilmMode(bool active)
    {
        isInFilmMode = active;
        currentMoveSpeed = active ? filmModeSpeed : normalMoveSpeed;
    }

    public void SetMovementInput(Vector2 input)
    {
        moveInput = input;
    }

    public void HandleLookInput(float mouseX, float mouseY)
    {
        mouseX *= mouseSensitivityX * Time.deltaTime * 10f;
        mouseY *= mouseSensitivityY * Time.deltaTime * 10f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        transform.position += move * currentMoveSpeed * Time.deltaTime;
    }

}