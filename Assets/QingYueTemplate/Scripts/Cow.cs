using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cow : MonoBehaviour
{

    // 創造 靜態方法
    public static Cow CreateCow(Transform FatherTransform, Transform pf_Cow, Vector3 spawnPosition)
    {
        Cow cow = Instantiate(pf_Cow, spawnPosition, Quaternion.identity).GetComponent<Cow>();
        cow.transform.SetParent(FatherTransform);
        cow.SetUp();
        return cow;
    }


    public event EventHandler OnDestory;
    public event EventHandler OnClick;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // 類在最下面 單純屬性
    private CowStat cowStat; 

    private bool isMoving;
    [SerializeField] private Vector3 followPosition;

    // 骰子 用於 判斷要移動 或 休息
    private float rollTimer;
    private float rollMaxTimer;

    private Button button;

    private void SetUp()
    { 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isMoving = false;
        followPosition = transform.position;
        rollMaxTimer = 2f;
        rollTimer = rollMaxTimer;

        cowStat = new CowStat(5 , UnityEngine.Random.Range(1, 10));

        button = GetComponent<Button>();
        button.onClick.AddListener(() => { OnClick?.Invoke(this, EventArgs.Empty); });
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
            isMoving = UnityEngine.Random.Range(0, 3) == 0;

            if (isMoving)
            {
                followPosition = new Vector3(
                    UnityEngine.Random.Range(-5f, 5f),
                    UnityEngine.Random.Range(-2f, -5f)
                );
            }
        }
    }

    private void MoveTo(Vector3 position)
    {
        spriteRenderer.flipX = position.x >= transform.position.x;

        Vector3 direction = (position - transform.position).normalized;
        float moveSpeed = 2f;
        float distance = Vector3.Distance(transform.position, position);

        if (distance > 0f)
        {
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            float newDistance = Vector3.Distance(newPosition, position);

            if (newDistance > distance)
            {
                newPosition = position;
            }

            transform.position = newPosition;
        }

        if (transform.position == position)
        {
            isMoving = false;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public int DamageBeforeGrabAmount(int value)
    {
        cowStat.Damage(value);
        int grabAmount = cowStat.GrabAmount();
        
        if (cowStat.Hp <= 0 || cowStat.CowMeatAmount <= 0)
        {
            
            OnDestory?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            
        }
        return grabAmount;
    }

    
    private class CowStat
    {

        public int Hp { get;private set; }
        public int CowMeatAmount { get;private set; }
        
        public CowStat(int Hp , int CowMeatAmount)
        {
            
            this.Hp = Hp;
            this.CowMeatAmount = CowMeatAmount;
        }
        
        public void Damage(int value)
        {
            Hp -= value;
        }

        public int GrabAmount()
        {
            
            
            int grabAmount = UnityEngine.Random.Range(1, 3);

            
            if (CowMeatAmount > grabAmount)
            {
                CowMeatAmount -= grabAmount;
            }
            else
            {
                grabAmount = CowMeatAmount;
                CowMeatAmount -= grabAmount;
            }
            
            return  grabAmount;
        }
    }
}
