using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    //di chuyen trai phai
    private float move;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    // Xet dk nhay
    private bool canJump;
    public Transform _canJump;
    public LayerMask Ground;
    private bool doubleJump;

    // Animation 
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

    //KnockBack
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Move();
        Jump();
        UpdateAnimationState();
        KnockBackCouter();

        Attack();
        UpdateAttackTransform();


    }

    public virtual void KnockBackCouter()
    {
        if (KBCounter <= 0)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }
        else
        {
            if (KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }
            KBCounter -= Time.deltaTime;
        }
    }
    // Thay doi AttackTransform theo huong nhan vat
    public void UpdateAttackTransform()
    {
        Vector3 attackTransformPosition = AttackTransform.position;

        Vector3 characterDirection = sprite.flipX ? Vector3.left : Vector3.right;

        Vector3 newAttackTransformPosition = transform.position + characterDirection * 2f;

        AttackTransform.position = new Vector3(newAttackTransformPosition.x, attackTransformPosition.y, attackTransformPosition.z);

        float characterRotation = sprite.flipX ? 180f : 0f;

        float attackRotation = sprite.flipX ? 180f : 0f;

        AttackTransform.rotation = Quaternion.Euler(0f, 0f, characterRotation + attackRotation);
    }


    // Di chuyen
    

    protected virtual void Move()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

    }

    protected virtual void Jump()
    {
        canJump = Physics2D.OverlapCircle(_canJump.position, 0.2f, Ground);

        if (canJump && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    // Attack
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
        if (objectsToHit.Length > 0)
        {
            Debug.Log("Hit");
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit
                    (damage, (transform.position - objectsToHit[i].transform.position).normalized, 100);
            }

        }
    }
}
