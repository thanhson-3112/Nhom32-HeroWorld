using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;


    [SerializeField] protected float health = 20f;
    [SerializeField] protected float recollLength = 0.2f;
    [SerializeField] protected float recollFactor = 3.5f;
    [SerializeField] protected bool isRecolling = false;
    protected float recollTimer;

    public int damage = 1;
    public PlayerLife playerLife;

    public PlayerDoubleJump playerDoubleJump;

    public float activationDistance = 3f;
    private Transform playerTransform;
    private bool isAttacking = false;

    public GameObject finishLevelObject;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("EnemyDeath");
            Destroy(gameObject, 1.3f);
            Time.timeScale = 1f;

            ShowNextMap();
        }
        else
        {
            HideNextMap();
        }

        //Danh quai lui lai
        if (isRecolling)
        {
            if (recollTimer < recollLength)
            {
                recollTimer += Time.deltaTime;
            }
            else
            {
                isRecolling = false;
                recollTimer = 0;
            }
        }

        //Boss tan cong
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= activationDistance && !isAttacking)
        {
            anim.SetTrigger("FlyingEyeAttack");
            isAttacking = true;
            anim.SetTrigger("FlyingEyeRun");
        }
        else
        {
            isAttacking = false;
            anim.SetTrigger("FlyingEyeRun");
        }

    }

    //Boss bi tan cong
    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        anim.SetTrigger("FlyingEyeTakeHit");
        anim.SetTrigger("FlyingEyeRun");
        if (!isRecolling)
        {
            rb.AddForce(-_hitForce * recollFactor * _hitDirection);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDoubleJump.KBCounter = playerDoubleJump.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerDoubleJump.KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerDoubleJump.KnockFromRight = false;
            }
            playerLife.TakeDamage(damage);

        }
    }

    private void ShowNextMap()
    {
        if (finishLevelObject != null)
        {
            finishLevelObject.SetActive(true);
        }
        else
        {
            Debug.LogError("NextMap GameObject is not assigned in the inspector.");
        }
    }

    private void HideNextMap()
    {
        if (finishLevelObject != null)
        {
            finishLevelObject.SetActive(false);
        }
        else
        {
            Debug.LogError("NextMap GameObject is not assigned in the inspector.");
        }
    }
}
