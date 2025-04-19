using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class Cow : MonoBehaviour
{

    // 創造 靜態方法
    public static Transform CreateCow(Transform pf_Cow, Vector3 spawnPosition , List<Transform> cows)
    {
        Cow cow = Instantiate(pf_Cow, spawnPosition, Quaternion.identity).GetComponent<Cow>();
        cow.SetUp();
        return cow.transform;
    }

    // 摧毀事件
    public event EventHandler OnDestory; 

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // 類在最下面 單純屬性
    private CowStat cowStat; 

    private bool isMoving;
    [SerializeField] private Vector3 followPosition;

    // 骰子 用於 判斷要移動 或 休息
    private float rollTimer;
    private float rollMaxTimer;
    
    

    private void SetUp()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isMoving = false;
        followPosition = transform.position;
        rollMaxTimer = 2f;
        rollTimer = rollMaxTimer;

        cowStat = new CowStat();
        cowStat.SetUp(3);
        
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

    public CowStat GetCowStat()
    {
        return cowStat;
    }

    public void Damage(int value)
    {
        cowStat.Hp -= value;
        if (cowStat.Hp <= 0)
        {
            
            OnDestory?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            return;
        }
    }

    public class CowStat
    {

        public int Hp { get; set; }

        public void SetUp(int Hp)
        {
            
            this.Hp = Hp;
        }


    }
}
