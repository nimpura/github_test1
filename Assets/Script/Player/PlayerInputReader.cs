using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Action Names")]
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string spawnActionName = "Spawn";
    [SerializeField] private string pauseActionName = "Pause";

    private InputAction moveAction;
    private InputAction spawnAction;
    private InputAction pauseAction;

    public event Action OnspawnAction;
    public event Action OnPauseAction;
    public bool IsInputBlocked { get; set; }

    public Vector2 MoveVector { get; private set; }

    private void Awake()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        ResolveActions();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (pauseAction.WasPerformedThisFrame())
        {
            Pause();
        }

        if (IsInputBlocked) return;

        MoveVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        if (spawnAction.WasPerformedThisFrame())
        {
            Spawn();
        }
    }

    private void OnEnable()
    {
        spawnAction.Enable();
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        spawnAction.Disable();
        pauseAction.Disable();
    }

    private void Spawn()
    {
        Debug.Log("Spawn ют╥б");
        OnspawnAction?.Invoke();
    }

    private void Pause()
    {
        OnPauseAction?.Invoke();
    }

    private void ResolveActions()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogWarning("[PlayerInputReader] playerInput Or actions is null");
            return;
        }

        moveAction = FindAction(moveActionName);
        spawnAction = FindAction(spawnActionName);
        pauseAction = FindAction(pauseActionName);
    }

    private InputAction FindAction(string actionName)
    {
        if (string.IsNullOrWhiteSpace(actionName))
        {
            Debug.Log("[PlayerInputReader] moveActionName is null or WhiteSpace");
            return null;
        }

        InputAction action = playerInput.actions.FindAction(actionName, false);
        if (action == null)
        {
            Debug.LogWarning($"[PlayerInputReader] Action not found : {actionName}");
            return null;
        }

        return action;
    }
}
