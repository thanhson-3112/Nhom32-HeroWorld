using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private Text Skill;
    [SerializeField] private AudioSource FinishSoundEffect;

    private bool levelCompleted = false;
    private void Start()
    {
        Skill.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !levelCompleted)
        {
            FinishSoundEffect.Play();
            Skill.gameObject.SetActive(true);
            Invoke("CompleteLevel", 2.5f);
            levelCompleted = true;
            
        }
    }

    public void CompleteLevel()
    {
        Skill.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
