using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    [Header("Direction")]
    [SerializeField] DIRECTION currentDirection;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 10.0f;

    private Rigidbody2D rb2D;
    private Vector2 inputDirection;

    public DIRECTION CurrentDirection => currentDirection;


    private void Awake()
    {
        if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentDirection = DIRECTION.LEFT;
    }

    private void Update()
    {
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = rb2D.position + inputDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        rb2D.MovePosition(newPosition);
    }

    public void Move(Vector2 direction)
    {
        inputDirection = direction;
    }

    private void UpdateDirection()
    {
        const float deadzoneValue = 0.1f;

        if (inputDirection.sqrMagnitude < 0.01f) return;

        float x = inputDirection.x;
        float y = inputDirection.y;

        if (x > deadzoneValue)
        {
            if (y > deadzoneValue) currentDirection = DIRECTION.RIGHTUP;
            else if (y < -1 * deadzoneValue) currentDirection = DIRECTION.RIGHTDOWN;
            else currentDirection = DIRECTION.RIGHT;
        }
        else if (x < -1 * deadzoneValue)
        {
            if (y > deadzoneValue) currentDirection = DIRECTION.LEFTUP;
            else if (y < -1 * deadzoneValue) currentDirection = DIRECTION.LEFTDOWN;
            else currentDirection = DIRECTION.LEFT;
        }
        else
        {
            if (y > deadzoneValue) currentDirection = DIRECTION.UP;
            else if (y < -1 * deadzoneValue) currentDirection = DIRECTION.DOWN;
        }
    }
}
