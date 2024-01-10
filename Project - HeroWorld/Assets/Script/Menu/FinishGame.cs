using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private AudioSource FinishSoundEffect;

    private bool gameCompleted = false;
    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !gameCompleted)
        {
            FinishSoundEffect.Play();
            Invoke("CompleteLevel", 1.5f);
            gameCompleted = true;

        }
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(5);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);

    }
}
