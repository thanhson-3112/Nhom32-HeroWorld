using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Vector3 respawnPoint;
    void Start()
    {
        respawnPoint = transform.position;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            respawnPoint = transform.position;
        }
    }
}
