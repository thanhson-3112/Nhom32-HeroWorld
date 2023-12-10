using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float move;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float dashForce = 8f;
    [SerializeField] private float dashDuration = 0.5f;
    private bool isDashing = false;
    private bool hasDashed = false; // Add this variable

    private bool canJump;
    public Transform _canJump;
    public LayerMask Ground;
    private bool doubleJump;

    private enum MovementState { idle, running, jumping, falling }
    private MovementState state = MovementState.idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
            Jump();
            UpdateAnimationState();
        }

        Attack();

        // Dash input detection
        if (Input.GetMouseButtonDown(1) && !isDashing && !hasDashed)
        {
            anim.SetTrigger("dash");
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        hasDashed = true; // Set to true when dashing
        
        Vector2 originalVelocity = rb.velocity;
        
        rb.velocity = new Vector2(rb.velocity.x + (sprite.flipX ? -dashForce : dashForce), rb.velocity.y);
        
        yield return new WaitForSeconds(dashDuration);
        
        rb.velocity = originalVelocity;
        isDashing = false;
    }

    protected virtual void Move()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Reset hasDashed when grounded
        if (canJump)
        {
            hasDashed = false;
        }
    }

    protected virtual void Jump()
    {
        canJump = Physics2D.OverlapCircle(_canJump.position, 0.2f, Ground);

        if (canJump && !Input.GetKey(KeyCode.Space))
        {
            doubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump || (doubleJump && !hasDashed))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = !doubleJump;
            }
        }
    }

    protected virtual void UpdateAnimationState()
    {
        this.state = MovementState.idle;

        if (move > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (move < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    protected virtual void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }
}
