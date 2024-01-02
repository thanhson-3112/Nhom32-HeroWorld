using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] movePoint;
    public float moveSpeed;
    public int moveDestination;

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
            
            if(transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(-6, 6, 6);
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(6, 6, 6);
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
        }

        else
        {
            if(Vector2.Distance(transform.position, playerTransform.position) < chaseDistance){
                isChasing = true;
                
            }

            if(moveDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePoint[0].position, moveSpeed * Time.deltaTime); 
                if(Vector2.Distance(transform.position, movePoint[0].position) < .2f)
                {
                    transform.localScale = new Vector3(6,6,6);
                    moveDestination = 1;
                }
            }
            if (moveDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePoint[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, movePoint[1].position) < .2f)
                {
                    transform.localScale = new Vector3(-6,6,6);
                    moveDestination = 0;
                }
            }
        }




        
    }
}
