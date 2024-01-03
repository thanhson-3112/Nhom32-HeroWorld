using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinishLevel : MonoBehaviour
{

    private bool levelCompleted = false;
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !levelCompleted)
        {
            Invoke("CompleteLevel", 1.5f);
            levelCompleted = true;
            
        }
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
