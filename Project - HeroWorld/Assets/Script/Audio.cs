using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource sceneMusic;
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private GameObject boss;

    private bool bossMusicPlaying = false;

    void Start()
    {
        sceneMusic.Play();
        bossMusic.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !bossMusicPlaying)
        {
            sceneMusic.Stop();

            if (boss != null)
            {
                bossMusic.Play();
                bossMusicPlaying = true;
            }
        }
    }

    private void Update()
    {
        if (boss == null && bossMusicPlaying)
        {
            bossMusic.Stop();
            bossMusicPlaying = false;

            sceneMusic.Play();
        }
    }
}
