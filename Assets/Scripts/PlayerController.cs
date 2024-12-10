using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.SceneView;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject cameraModel;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float cameraPitch;

    private bool isCameraMode;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnModeChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameModeManager.Instance.ToggleMode();
        }
    }

    private void Start()
    {
        GameModeManager.Instance.OnGameModeChanged.AddListener(HandleModeChanged);
    }

    private void HandleModeChanged(GameModeManager.GameMode newMode)
    {
        isCameraMode = newMode == GameModeManager.GameMode.Camera;
        cameraUI.SetActive(isCameraMode);
        //cameraModel.SetActive(isCameraMode);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void HandleLook()
    {
        // Horizontal rotation (Player rotation)
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        // Vertical rotation (Camera pitch)
        cameraPitch -= lookInput.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }

    private void Attack()
    {
        GameModeManager.Instance.TakingAPicture();
    }
}