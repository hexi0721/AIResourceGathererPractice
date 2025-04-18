using UnityEngine;
using UnityEngine.UIElements;

public class SwordMan : MonoBehaviour , IUnit
{

    Animator animator;
    Rigidbody2D rgbd2D;

    bool attackDone;
    bool isMoving;
    Vector3 moveDir;

    const float MOVE_SPEED = 5f;



    

    private void Start()
    {
        
        animator = GetComponent<Animator>();
        rgbd2D = GetComponent<Rigidbody2D>();

        attackDone = true;
        isMoving = false;
        moveDir = Vector3.zero;

    }

    
    private void Update()
    {
        // HandleManualAnimator();



    }

    private void HandleManualAnimator()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;

        }

        if (Input.GetKey(KeyCode.S))
        {

            moveY = -1f;

        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;

        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;

        }


        if (Input.GetKeyDown(KeyCode.Space) && attackDone)
        {
            animator.SetTrigger("Attack");
            attackDone = false; // 設置為 false，防止重複觸發
        }

        moveDir = new Vector2(moveX, moveY).normalized;

        isMoving = moveDir.x != 0 || moveDir.y != 0;

        if (isMoving)
        {

            animator.SetFloat("movingFloatForAttack", 1f);
            animator.SetFloat("Horizontal", moveDir.x);
            animator.SetFloat("Vertical", moveDir.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            rgbd2D.linearVelocity = Vector2.zero;
            animator.SetFloat("movingFloatForAttack", 0f);
            animator.SetBool("isMoving", false);
        }

        // 檢查動畫是否結束
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackBlendTree") && stateInfo.normalizedTime >= .99f) // stateInfo.normalizedTime 接近 1
        {
            attackDone = true;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rgbd2D.linearVelocity = moveDir * MOVE_SPEED;
        }
        else
        {
            rgbd2D.linearVelocity = Vector2.zero;
        }
    }

    public bool IsIdle()
    {
        return !isMoving;
    }

    public void MoveTo(Vector3 position)
    {
        
    }

    public void PlayAnimation()
    {
        
    }
}

