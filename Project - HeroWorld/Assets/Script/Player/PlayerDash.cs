using System.Collections;
using System.Collections.Generic;
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
    private bool hasDashed = false; 

    //Jump
    private bool canJump;
    public Transform _canJump;
    public LayerMask Ground;
    private bool doubleJump;

    //Animation
    private enum MovementState { idle, running, jumping, falling }
    private MovementState state = MovementState.idle;

    // Attack
    bool attack = false;
    private float timeBetweenAttack = 0.5f;
    private float timeSinceAttack;
    [SerializeField] Transform AttackTransform;
    [SerializeField] Vector2 AttackArea;
    [SerializeField] LayerMask attackablelayer;
    [SerializeField] float damage;


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
        UpdateAttackTransform();
    }

    public void UpdateAttackTransform()
    {

        // L?y v? tr� hi?n t?i c?a AttackTransform
        Vector3 attackTransformPosition = AttackTransform.position;

        // X�c ??nh h??ng m?t c?a nh�n v?t
        Vector3 characterDirection = sprite.flipX ? Vector3.left : Vector3.right;

        // T�nh to�n v? tr� m?i c?a AttackTransform
        Vector3 newAttackTransformPosition = transform.position + characterDirection * 2f;

        // G�n v? tr� m?i cho AttackTransform, gi? nguy�n tr?c y
        AttackTransform.position = new Vector3(newAttackTransformPosition.x, attackTransformPosition.y, attackTransformPosition.z);

        // X�c ??nh g�c quay d?a tr�n h??ng c?a nh�n v?t
        float characterRotation = sprite.flipX ? 180f : 0f;

        // X�c ??nh g�c quay c?a AttackTransform
        float attackRotation = sprite.flipX ? 180f : 0f;

        // G�n g�c quay cho AttackTransform
        AttackTransform.rotation = Quaternion.Euler(0f, 0f, characterRotation + attackRotation);
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackTransform.position, AttackArea);
    }
    protected virtual void Attack()
    {
        attack = Input.GetMouseButtonDown(0);
        timeSinceAttack += Time.deltaTime;

        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("attack");
            Hit(AttackTransform, AttackArea);
        }
    }

    private void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackablelayer);
        if(objectsToHit.Length > 0)
        {
            Debug.Log("Hit");
        }
        for(int i =0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit
                    (damage, (transform.position - objectsToHit[i].transform.position).normalized,100);
            }
            
        }
    }
}
