using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    private Rigidbody2D rb;

    [SerializeField] protected float health;
    [SerializeField] protected float recollLength = 0.2f;
    [SerializeField] protected float recollFactor = 2f;
    [SerializeField] protected bool isRecolling = false;
    protected float recollTimer;

    public int damage;
    public PlayerLife playerLife;

    public PlayerDash playerDash;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        if (isRecolling)
        {
            if(recollTimer < recollLength)
            {
                recollTimer +=Time.deltaTime;
            }
            else
            {
                isRecolling = false;
                recollTimer = 0;
            }
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce )
    {
        health -= _damageDone;
        if (!isRecolling)
        {
            rb.AddForce(- _hitForce * recollFactor * _hitDirection);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerDash.KBCounter = playerDash.KBTotalTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                playerDash.KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerDash.KnockFromRight = false;
            }
            playerLife.TakeDamage(damage);
        }
    }
}
