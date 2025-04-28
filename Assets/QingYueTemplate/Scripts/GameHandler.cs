using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    static GameHandler instance;

    [Header("��")]
    [SerializeField] Cow pf_Cow;
    [SerializeField] Cow pf_Calf;
    List<Cow> pfList;

    [Header("�ܮw")]
    [SerializeField] Transform storage;

    [Header("�D��")]
    [SerializeField] SwordMan swordMan;

    // �n�ǵ���L������
    [SerializeField] Cow tf;

    // ���s
    List<Cow> cows;

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
        Meat meat = new Meat();
        pfList = new List<Cow>() { pf_Cow, pf_Calf };

        tf = null;
        unit = swordMan.GetComponent<IUnit>();

        cows = new List<Cow>();
        SpawnCow();
        

        swordMan.OnArrivedSotrage += SwordMan_OnArrivedStorage;
        SwordMan_OnArrivedStorage(this , EventArgs.Empty); // �o�̥�Ĳ�o�F �ҥH�@�}�l�~�������itf
        
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
        Cow returenCow = null;
        if (cows.Count == 0)
        {
            SpawnCow();
        }

        foreach (Cow cow in cows)
        {

            float distanceToCow = Vector3.Distance(cow.transform.position, swordMan.transform.position);

            if (distanceToCow < distance)
            {
                distance = distanceToCow;
                returenCow = cow;

            }
        }

        tf = cows[cows.IndexOf(returenCow)];
    }


    /// <summary>
    /// �ͦ���
    /// </summary>
    private void SpawnCow()
    {
        if (cows.Count < 5)
        {

            Cow animal = pfList[UnityEngine.Random.Range(0 ,pfList.Count)];

            Cow cow = Cow.CreateCow(animal.transform, new Vector3(UnityEngine.Random.Range(-5f, 5f),UnityEngine.Random.Range(-2f, -5f)));

            cow.GetComponent<Cow>().OnDestory += Cow_OnDestory;
            cows.Add(cow);
        }

    }

    // ���Q�ʷ��n������
    private void Cow_OnDestory(object sender, EventArgs e)
    {
        cows.Remove(tf);
        SearchNextCow();

    }

    public static Cow GetCowPosition_Static()
    {
        return instance.tf;
    }

    public static Vector3 GetStoragePosition_Static()
    {
        return instance.storage.position;
    }
    

    


}
