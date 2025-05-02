using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using static IUnit;

public class SwordMan : MonoBehaviour , IUnit
{

    public event EventHandler OnArrivedSotrage;
    public event EventHandler OnAlreadySlayCow;
    public event EventHandler OnClick;

    Animator animator;
    AnimatorStateInfo stateInfo;

    // AI不會用到 手動要用的
    Rigidbody2D rgbd2D;

    Button button;

    bool AttackDone;
    bool isMoving;

    // 是否鎖定目標 是的話繼續追牛 否的話代表回到倉庫
    public bool TargetLock { get; set; }

    Vector3 moveDir;
    const float MOVE_SPEED = 5f;
    Vector3 position;

    public Stat UnitStat { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbd2D = GetComponent<Rigidbody2D>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { OnClick?.Invoke(this, EventArgs.Empty); });

        AttackDone = true;
        isMoving = false;
        TargetLock = false;
        moveDir = Vector3.zero;
        position = transform.position;

        UnitStat = new Stat() { ATK = 2, 背負肉的數量 = 0 };
        
    }

    private void Start()
    {
        
    }
    /*
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
    */

    private void Update()
    {
        // HandleManualAnimator();
        HandleMovement();

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


        if (Input.GetKeyDown(KeyCode.Space) && AttackDone)
        {
            animator.SetTrigger("Attack");
            AttackDone = false; // 設置為 false，防止重複觸發
        }

        moveDir = new Vector2(moveX, moveY).normalized;

        isMoving = moveDir.x != 0 || moveDir.y != 0;

        if (isMoving)
        {
            rgbd2D.linearVelocity = moveDir * MOVE_SPEED;
            animator.SetFloat("AttackOrWalkAttack" , 1f);
            
            animator.SetFloat("Horizontal", moveDir.x);
            animator.SetFloat("Vertical", moveDir.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            rgbd2D.linearVelocity = Vector2.zero;
            animator.SetFloat("AttackOrWalkAttack", 0f);
            animator.SetBool("isMoving", false);
        }

        // 檢查動畫是否結束
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("AttackBlenderTree") && stateInfo.normalizedTime >= .99f) // stateInfo.normalizedTime 接近 1
        {
            AttackDone = true;
        }
    }

    private void HandleMovement()
    {

        Vector3 dir = (position - transform.position).normalized;
        float moveSpeed = 2f;
        float distance = Vector3.Distance(transform.position, position);

        if (distance >= 0f)
        {
            Vector3 newPosition = transform.position + dir * moveSpeed * Time.deltaTime;
            float newDistance = Vector3.Distance(newPosition, position);

            if (newDistance > distance)
            {
                newPosition = position;
            }

            transform.position = newPosition;

        }
    }


    /// <summary>
    /// 判斷是不是在 Idle 動畫
    /// </summary>
    public bool IsIdle()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("IdleBlenderTree");
    }

    public void MoveTo(Vector3 stopPosition , float stopDistance , Action action)
    {
        if (stopPosition.x > transform.position.x)
        {
            animator.SetFloat("Horizontal", 1f);
        }
        else if (stopPosition.x <= transform.position.x)
        {
            animator.SetFloat("Horizontal", -1f);
        }

        if(stopPosition.y > transform.position.y)
        {
            animator.SetFloat("Vertical", 1f);
        }
        else if (stopPosition.y <= transform.position.y)
        {
            animator.SetFloat("Vertical", -1f);
        }

        animator.SetBool("isMoving", true);
        isMoving = true;

        Vector3 dir = (stopPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, stopPosition);
        distance -= stopDistance;
        position = transform.position + dir * distance;
        StartCoroutine(MoveToCoroutine(action));

    }

    private IEnumerator MoveToCoroutine(Action action)
    {
        
        while (transform.position != position)
        {
            yield return null;
        }

        if (!TargetLock)
        {
            OnArrivedSotrage?.Invoke(this, EventArgs.Empty);
        }

        animator.SetBool("isMoving", false);
        isMoving = false;

        action?.Invoke();

    }

    Action onAnimationCompleted;
    // 播放揮砍動畫
    public void PlaySlayAnimation(Transform LookAt, Action onAnimationCompleted)
    {
        Vector3 LookAtPostion = LookAt.position;
        if (LookAtPostion.x > transform.position.x)
        {
            animator.SetFloat("Horizontal", 1f);
        }
        else if (LookAtPostion.x <= transform.position.x)
        {
            animator.SetFloat("Horizontal", -1f);
        }

        if (LookAtPostion.y > transform.position.y)
        {
            animator.SetFloat("Vertical", 1f);
        }
        else if (LookAtPostion.y <= transform.position.y)
        {
            animator.SetFloat("Vertical", -1f);
        }

        AttackDone = false;
        animator.SetTrigger("Attack");
        StartCoroutine(Cow_OnDamaged_Coroutine(LookAt));

        this.onAnimationCompleted = onAnimationCompleted;

    }


    // 揮砍的動畫完整時間是1f 砍的時間是0.5 回饋時間 0.25
    private IEnumerator Cow_OnDamaged_Coroutine(Transform LooAt)
    {

        yield return new WaitForSeconds(.5f);
        LooAt.GetComponent<SpriteRenderer>().color = Color.red;

        UnitStat.背負肉的數量 += LooAt.GetComponent<Cow>().DamageBeforeGrabAmount(UnityEngine.Random.Range(1 , 2));

        // 傷害完 牛有可能死了
        yield return new WaitForSeconds(.25f);
        if(LooAt == null)
        {
            OnAlreadySlayCow?.Invoke(this, EventArgs.Empty);
        }

        if (LooAt != null)
        {
            
            LooAt.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void PlaySlayAnimationEnd()
    {
        onAnimationCompleted?.Invoke();
        onAnimationCompleted = null;
        AttackDone = true;
    }

}

