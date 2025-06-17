using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Value")]
    Vector3 _moveDir;
    Vector3 _atkDir;
    public bool StopMove = false;
    float _currentSpeed = 0f;

    [Header("Components")]
    [SerializeField] PlayerStat _stat;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Animator _animator;

    [Header("Input")]
    [SerializeField] PlayerInput _input;
    InputAction _moveAction;
    InputAction _attackAction;
    InputAction _utilityAction;
    InputAction _defenseAction;
    InputAction _interactAction;

    void OnEnable()
    {
        _moveAction = _input.actions["Move"];
        _attackAction = _input.actions["Attack"];
        _utilityAction = _input.actions["Utility"];
        _defenseAction = _input.actions["Defend"];
        _interactAction = _input.actions["Interact"];

        _moveAction.performed += ctx => OnMove(ctx.ReadValue<Vector2>().normalized);
        _moveAction.canceled += ctx => OnMove(Vector2.zero);

        _attackAction.performed += _ => OnAttack();
        _utilityAction.performed += _ => OnUtility();
        _defenseAction.performed += _ => OnDefend();
        _interactAction.performed += _ => OnInteract();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        _rb.linearVelocity = _moveDir * _stat.GetStat(StatType.Speed);
        Vector3 flatDir = new Vector3(_moveDir.x, 0f, _moveDir.z);
        float targetSpeed = 0f;

        if (flatDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
            targetSpeed = 3f;
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, 0.2f);
        _animator.SetFloat("Speed", _currentSpeed);
    }

    void OnMove(Vector2 input)
    {
        _moveDir = new Vector3(input.x, _rb.linearVelocity.y, input.y);
    }

    public void OnAttack()
    {
        GetMouseDirection();
    }

    public void OnUtility()
    {
        GetMouseDirection();
    }

    public void OnDefend()
    {
        GetMouseDirection();
    }

    public void OnInteract()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000, LayerMask.GetMask("Interactable")))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }

    void GetMouseDirection()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000, LayerMask.GetMask("Ground")))
            _atkDir = (hit.point - transform.position).normalized;
    }

    public void OnFootstep()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + _atkDir * 10f);
    }
}
