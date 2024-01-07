using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] public int maxHealth = 5;
    [SerializeField] public int health;

    [SerializeField] public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    //CHeckPoint
    private Vector2 respawnPoint;
    public GameObject startPoint;

    //Sound
    [SerializeField] private AudioSource DamageSoundEffect;
    [SerializeField] private AudioSource DeathSoundEffect;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        respawnPoint = startPoint.transform.position;
    }

    public void Update()
    {
        // Dieu chinh thanh mau
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i< health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if(i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        DamageSoundEffect.Play();
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (DeathSoundEffect != null)
        {
            DeathSoundEffect.Play();
        }

        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");

        Invoke("Respawn", 1.7f); 
    }

    private void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = respawnPoint;
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Blood"))
        {
            if(health != maxHealth)
            {
                Destroy(collision.gameObject);
                health++;
            }
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            maxHealth++;
            health = maxHealth;
        }



        if (collision.CompareTag("CheckPoint"))
        {
            respawnPoint = collision.transform.position;
            Debug.Log("Checkpoint reached. Respawn point updated to: " + respawnPoint);
        }
    }
    

    private void RestartLevel()
    {
        transform.position = respawnPoint;
        health = maxHealth;
    }


}
