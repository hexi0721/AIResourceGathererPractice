using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    static GameHandler instance;

    [Header("��")]
    [SerializeField] Transform pf_Cow;
    [SerializeField] Transform pf_Calf;
    List<Transform> pfList;

    [Header("�ܮw")]
    [SerializeField] Transform storage;

    [Header("�D��")]
    [SerializeField] SwordMan swordMan;

    // �n�ǵ���L������
    Transform tf;

    // ���s
    [SerializeField] List<Transform> cows;

    IUnit unit;
    
    // �ͦ��ɶ�
    float spawnTimer;
    float spawnTimerMax;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pfList = new List<Transform>() { pf_Cow, pf_Calf };

        tf = null;
        unit = swordMan.GetComponent<IUnit>();

        cows = new List<Transform>();
        SpawnCow();
        

        swordMan.OnArrivedSotrage += SwordMan_OnArrivedStorage;
        SwordMan_OnArrivedStorage(this , EventArgs.Empty);
        
        spawnTimerMax = 5f;
        spawnTimer = spawnTimerMax;
    }

    private void Update()
    {
        
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer += spawnTimerMax;
            SpawnCow();
        }

    }

    // ��F�ܮw�Ӱ�����
    private void SwordMan_OnArrivedStorage(object sender, EventArgs e)
    {
        SearchNextCow();
        unit.TargetLock = true;

    }

    /// <summary>
    /// �j�M�̪񪺤�
    /// </summary>
    private void SearchNextCow()
    {
        float distance = float.MaxValue;
        Transform returenTransform = null;
        if (cows.Count == 0)
        {
            SpawnCow();
        }

        foreach (var cowPostion in cows)
        {

            float distanceToCow = Vector3.Distance(cowPostion.position, swordMan.transform.position);

            if (distanceToCow < distance)
            {
                distance = distanceToCow;
                returenTransform = cowPostion;

            }
        }

        tf = cows[cows.IndexOf(returenTransform)];
    }


    /// <summary>
    /// �ͦ���
    /// </summary>
    private void SpawnCow()
    {
        if (cows.Count < 5)
        {

            Transform animal = pfList[UnityEngine.Random.Range(0 ,pfList.Count)];

            Transform cow = Cow.CreateCow(animal, new Vector3(
                UnityEngine.Random.Range(-5f, 5f),
                UnityEngine.Random.Range(-2f, -5f)
            ) , cows);

            cow.GetComponent<Cow>().OnDestory += Cow_OnDestory;
            cows.Add(cow);
        }

    }

    // ���Q�ʷ��n������
    private void Cow_OnDestory(object sender, EventArgs e)
    {
        cows.Remove(tf.transform);
        SearchNextCow();

    }

    public static Transform GetCowPosition_Static()
    {
        return instance.tf;
    }

    public static Vector3 GetStoragePosition_Static()
    {
        return instance.storage.position;
    }
    

    


}
