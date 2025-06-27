using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Modern input controller using Unity's Input System with proper event handling
/// Supports both touch and traditional input methods
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public sealed class PlayerInputController : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private float touchSensitivity = 1.0f;
    [SerializeField] private float deadZone = 0.1f;

    // Properties
    public Vector2 MoveInput { get; private set; }
    public Vector2 RawMoveInput { get; private set; }
    public bool IsInputActive { get; private set; }

    // Events
    public static event Action<Vector2> OnMoveInputChanged;
    public static event Action OnInputStarted;
    public static event Action OnInputStopped;

    private Controls controls;
    private Camera mainCamera;
    private Vector2 lastTouchPosition;
    private bool isTouchActive;

    private void Awake()
    {
        controls = new Controls();
        mainCamera = Camera.main;

        // Ensure we have a camera reference
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
            if (mainCamera == null)
            {
                Debug.LogWarning("PlayerInputController: No main camera found in the scene.");
            }
        }
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    private void EnableInputs()
    {
        controls.Player.Enable();

        // Keyboard/Gamepad input
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

        // Touch input
        controls.Player.TouchPosition.performed += OnTouchPositionChanged;
        controls.Player.TouchPress.started += OnTouchStarted;
        controls.Player.TouchPress.canceled += OnTouchEnded;
    }

    private void DisableInputs()
    {
        if (controls?.Player != null)
        {
            controls.Player.Move.performed -= OnMovePerformed;
            controls.Player.Move.canceled -= OnMoveCanceled;
            controls.Player.TouchPosition.performed -= OnTouchPositionChanged;
            controls.Player.TouchPress.started -= OnTouchStarted;
            controls.Player.TouchPress.canceled -= OnTouchEnded;

            controls.Player.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        SetMoveInput(context.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        SetMoveInput(Vector2.zero);
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        isTouchActive = true;
        lastTouchPosition = GetWorldTouchPosition();
        OnInputStarted?.Invoke();
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        isTouchActive = false;
        SetMoveInput(Vector2.zero);
    }

    private void OnTouchPositionChanged(InputAction.CallbackContext context)
    {
        if (!isTouchActive)
        {
            return;
        }

        Vector2 currentTouchPosition = GetWorldTouchPosition();
        Vector2 touchDelta = (currentTouchPosition - lastTouchPosition) * touchSensitivity;

        // Convert touch delta to movement input
        SetMoveInput(touchDelta.normalized);
        lastTouchPosition = currentTouchPosition;
    }

    private Vector2 GetWorldTouchPosition()
    {
        Vector2 screenPosition = controls.Player.TouchPosition.ReadValue<Vector2>();
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }

    private void SetMoveInput(Vector2 input)
    {
        RawMoveInput = input;
        // Apply dead zone
        MoveInput = input.magnitude > deadZone ? input : Vector2.zero;

        // Notify listeners
        OnMoveInputChanged?.Invoke(MoveInput);

        // Track previous state for input events
        bool wasInputActive = IsInputActive;
        IsInputActive = MoveInput.magnitude > deadZone;

        if (IsInputActive && !wasInputActive)
        {
            OnInputStarted?.Invoke();
        }
        else if (!IsInputActive && wasInputActive)
        {
            OnInputStopped?.Invoke();
        }
    }

    /// <summary>
    /// Get movement input with optional smoothing
    /// </summary>
    public Vector2 GetMovementInput(bool normalized = true)
    {
        return normalized ? MoveInput.normalized : MoveInput;
    }

    private void OnDestroy()
    {
        // Only dispose if Controls implements IDisposable
#if UNITY_INPUT_SYSTEM_EXISTS
        if (controls is IDisposable disposable)
        {
            disposable.Dispose();
        }
#endif
    }
}