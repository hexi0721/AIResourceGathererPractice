using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class Cow : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    [SerializeField] bool isMoving;
    [SerializeField] Vector3 followPosition;

    float rollTimer;
    float rollMaxTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isMoving = false;
        followPosition = transform.position;
        rollMaxTimer = 2f;
        rollTimer = rollMaxTimer;
    }

    private void Update()
    {

        if (isMoving)
        {
            animator.SetBool("isMoving", true);
            MoveTo(followPosition);
            
        }
        else
        {
            animator.SetBool("isMoving", false);
            Rest();
        }

        
    }

    private void Rest()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0f)
        {
            rollTimer += rollMaxTimer;
            isMoving = UnityEngine.Random.Range(0, 3) == 0 ? true : false;

            if (isMoving)
            {
                followPosition = new Vector3(UnityEngine.Random.Range(-17f, 17f), UnityEngine.Random.Range(-9f, 9f));
            }
        }
    }


    private void MoveTo(Vector3 position)
    {



        spriteRenderer.flipX = (position.x >= transform.position.x) ? true : false;

        Vector3 dir = (position - transform.position).normalized;
        float moveSpeed = 2f;
        float distance = Vector3.Distance(transform.position, position);

        if(distance >= 0f)
        {
            Vector3 newPosition = transform.position + dir * moveSpeed * Time.deltaTime;
            float newDistance = Vector3.Distance(newPosition, position);

            if(newDistance > distance)
            {
                newPosition = position;
            }

            transform.position = newPosition;

        }

        if(transform.position == position)
        {
            isMoving = false;
        }
        

    }
}
