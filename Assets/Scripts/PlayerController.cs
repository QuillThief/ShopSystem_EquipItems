using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private const string ISMOVING = "isMoving";
    private bool freezePlayer;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float collisionOffset = 0.02f;

    public ContactFilter2D movementFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!freezePlayer)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success) success = TryMove(new Vector2(movementInput.x, 0));
                if (!success) success = TryMove(new Vector2(0, movementInput.y));

                animator.SetBool(ISMOVING, success);
            }
            else
            {
                animator.SetBool(ISMOVING, false);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else return false;
        }
        else return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    public void SetFreezePlayer(bool fp)
    {
        freezePlayer = fp;
        if (fp) animator.SetBool(ISMOVING, false);
    }

}
