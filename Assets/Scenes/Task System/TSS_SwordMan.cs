using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class TSS_SwordMan : MonoBehaviour , TSS_ISwordMan
{

    public static TSS_SwordMan Create(Transform pf_swordMan , Vector3 spawnPosition)
    {
        TSS_SwordMan swordMan = Instantiate(pf_swordMan , spawnPosition , Quaternion.identity).GetComponent<TSS_SwordMan>();
        swordMan.SetUp();

        return swordMan;

    }

    Animator animator;
    AnimatorStateInfo stateInfo;

    //bool AttackDone;
    

    // �O�_��w�ؼ� �O�����~��l�� �_���ܥN��^��ܮw
    public bool TargetLock { get; set; }

    Vector3 position;

    public event EventHandler OnArrivedSotrage;
    
    private void SetUp()
    {
        animator = GetComponent<Animator>();

        // AttackDone = true;
        
        TargetLock = false;
        position = transform.position;
    }

    private void Update()
    {

        HandleMovement();

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
    /// �P�_�O���O�b Idle �ʵe
    /// </summary>
    public bool IsIdle()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("IdleBlenderTree");
    }

    public void MoveTo(Vector3 stopPosition , Action action)
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
        

        position = stopPosition;

        StartCoroutine(MoveToCoroutine(action));

    }

    private IEnumerator MoveToCoroutine(Action action)
    {
        
        while (transform.position != position)
        {
            yield return null;
        }

        if(TargetLock)
        {
            TargetLock = false;
            OnArrivedSotrage?.Invoke(this, EventArgs.Empty);
            
        }
        
        animator.SetBool("isMoving", false);
        

        action?.Invoke();

    }

    Action onAnimationCompleted;
    // ���񴧬�ʵe
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

        //AttackDone = false;
        animator.SetTrigger("Attack");
        StartCoroutine(Cow_OnDamaged_Coroutine(LookAt));


        this.onAnimationCompleted = onAnimationCompleted;

    }


    // ���媺�ʵe����ɶ��O1f �媺�ɶ��O0.5 �^�X�ɶ� 0.25
    private IEnumerator Cow_OnDamaged_Coroutine(Transform LooAt)
    {

        yield return new WaitForSeconds(.5f);
        LooAt.GetComponent<SpriteRenderer>().color = Color.red;

        LooAt.GetComponent<Cow>().DamageBeforeGrabAmount(2);
        yield return new WaitForSeconds(.25f);

        // �ˮ`�� �����i�঺�F
        if (LooAt != null)
        {
            
            LooAt.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void PlaySlayAnimationEnd()
    {

        onAnimationCompleted?.Invoke();
        onAnimationCompleted = null;
        //AttackDone = true;

    }

    public void PlayIdle2Animation(Action onAnimationCompleted)
    {
        animator.SetBool("isPlayIdle2", true);

        this.onAnimationCompleted = onAnimationCompleted;
        StartCoroutine(PlayIdle2AnimationEnd());
    }

    private IEnumerator PlayIdle2AnimationEnd()
    {
        yield return new WaitForSeconds(.5f);
        onAnimationCompleted?.Invoke();
        onAnimationCompleted = null;
        animator.SetBool("isPlayIdle2", false);
    }
}

