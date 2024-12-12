using static GameModeManager;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse Sensitivity")]
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityX = 8f;  // �¿� ����
    [SerializeField][Range(1f, 20f)] private float mouseSensitivityY = 8f;  // ���� ����

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
        // X��� Y�� ������ ���������� ����
        mouseX *= mouseSensitivityX;
        mouseY *= mouseSensitivityY;

        // deltaTime ���� - �����Ӱ� �����ϰ� ������ ȸ�� �ӵ� ����
        mouseX *= Time.deltaTime * 10f;  // ��ü���� ȸ�� ������ �� �� �ε巴�� �ϱ� ���� 10�� ���մϴ�
        mouseY *= Time.deltaTime * 10f;

        // ���� ȸ�� ���� ��� �� ����
        xRotation -= mouseY;  // ���콺 ���� ������ �ʿ���ٸ� += �� ����
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ī�޶� Ȧ���� ���� ȸ�� ����
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // �÷��̾� �¿� ȸ��
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