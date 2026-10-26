using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float currentSpeed;
    public static float walkSpeed = 5f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private float maxLookAngle = 85f;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.6f, 0f);

    public static float zoneRadius = 0f;
    [SerializeField] private float magnetStrength = 15f;
    [SerializeField] private float _wertivlYMagnet = 0.6f;

    [Header("Magnet Visuals")]
    [SerializeField] private Transform magnetVisualTransform;

    private Vector2 _move;
    private Vector2 _look;
    private bool _isGrounded;
    private float _pitch;

    private void Start()
    {
        currentSpeed = walkSpeed;
        _rigidbody.useGravity = true;

        if (_playerCamera != null)
        {
            _pitch = _playerCamera.transform.eulerAngles.x;
            if (_pitch > 180f) _pitch -= 360f;
        }

        UpdateMagnetVisual();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        _isGrounded = CheckGrounded();

        Vector3 move = GetForward() * _move.y + GetRight() * _move.x;
        Vector3 horizontalVelocity = move * currentSpeed;
        float verticalVelocity = _rigidbody.linearVelocity.y;

        _rigidbody.linearVelocity = new Vector3(
            horizontalVelocity.x,
            verticalVelocity,
            horizontalVelocity.z
        );

        transform.Rotate(Vector3.up, _look.x * mouseSensitivity);

        if (_playerCamera != null)
        {
            _pitch -= _look.y * mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, -maxLookAngle, maxLookAngle);
            _playerCamera.transform.eulerAngles = new Vector3(_pitch, transform.eulerAngles.y, 0f);
        }

        ApplyMagnet();
    }

    private void LateUpdate()
    {
        if (_playerCamera != null)
        {
            _playerCamera.transform.position = transform.position + cameraOffset;
        }

        UpdateMagnetVisual();
    }

    private bool CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            return hit.collider.CompareTag(groundTag);
        }
        return false;
    }

    private Vector3 GetForward()
    {
        Vector3 forward = _playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetRight()
    {
        Vector3 right = _playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void ApplyMagnet()
    {
        if (zoneRadius <= 0f) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, zoneRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Coin"))
            {
                Transform coinTransform = hitCollider.transform;
                Vector3 targetPosition = transform.position + Vector3.up * 0.5f;

                coinTransform.position = Vector3.MoveTowards(
                    coinTransform.position,
                    targetPosition,
                    magnetStrength * Time.deltaTime
                );
            }
        }
    }

    private void UpdateMagnetVisual()
    {
        if (magnetVisualTransform == null) return;

        float diameter = zoneRadius * 2f;
        magnetVisualTransform.localScale = new Vector3(diameter, _wertivlYMagnet, diameter);
    }
}