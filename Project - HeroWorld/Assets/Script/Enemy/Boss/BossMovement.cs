using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Transform[] movePoint;
    public float moveSpeed = 4f;
    public int moveDestination = 1;

    //Follow player
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {

            if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance)
            {
                isChasing = false;
            }
            else
            {
                if (transform.position.x > playerTransform.position.x)
                {
                    transform.localScale = new Vector3(-9, 9, 9);
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }
                if (transform.position.x < playerTransform.position.x)
                {
                    transform.localScale = new Vector3(9, 9, 9);
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }
            }
        }

        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;

            }

            if (moveDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePoint[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, movePoint[0].position) < .2f)
                {
                    transform.localScale = new Vector3(9, 9, 9);
                    moveDestination = 1;
                }
            }
            if (moveDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePoint[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, movePoint[1].position) < .2f)
                {
                    transform.localScale = new Vector3(-9, 9, 9);
                    moveDestination = 0;
                }
            }
        }





    }
}
