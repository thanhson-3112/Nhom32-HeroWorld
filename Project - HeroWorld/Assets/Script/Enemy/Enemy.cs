using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float recollLength = 0.2f;
    [SerializeField] float recollFactor = 2f;
    [SerializeField] bool isRecolling = false;

    float recollTimer;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce )
    {
        health -= _damageDone;
        if (!isRecolling)
        {
            rb.AddForce(- _hitForce * recollFactor * _hitDirection);
        }
    }
}
