using System;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour
{
    static GameHandler instance;

    [Header("��")]
    [SerializeField] Cow pf_Cow;
    [SerializeField] Cow pf_Calf;
    List<Cow> pfList;

    [Header("�ܮw")]
    [SerializeField] Transform storage;


    [SerializeField] List<SwordMan> swordMans;
    SwordManAI selectSwordMan;


    // ���s
    List<Cow> cows;

    // �ͦ��ɶ�
    float spawnTimer;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Meat meat = new Meat();
        pfList = new List<Cow>() { pf_Cow, pf_Calf };

        cows = new List<Cow>();
        SpawnCow();

        // swordMans = new List<SwordMan>();
        foreach (SwordMan swordMan in swordMans)
        {
            swordMan.OnArrivedSotrage += SwordMan_OnArrivedStorage;
            swordMan.OnAlreadySlayCow += SwordMan_OnAlreadySlayCow;
            swordMan.OnClick += SwordMan_OnClick;
        }

        
        // SearchNextCow(); // �o�̥�Ĳ�o�F �ҥH�@�}�l�~�������itf
        

        
    }

    private void Update()
    {
        
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {

            float spawnTimerMax = 5f;
            spawnTimer += spawnTimerMax;
            SpawnCow();
        }

    }

    // ��F�ܮw�Ӱ�����
    private void SwordMan_OnArrivedStorage(object sender, EventArgs e)
    {
        // SearchNextCow();

    }

    private void SwordMan_OnClick(object sender, EventArgs e)
    {
        
        SwordMan swordMan = sender as SwordMan;
        SwordManAI swordManAI = swordMan.GetComponent<SwordManAI>();
        if(selectSwordMan != null)
        {
            selectSwordMan.HideSelectCircle();
        }

        selectSwordMan = swordManAI;
        
        selectSwordMan.ShowSelectCircle();

    }

    private void SwordMan_OnAlreadySlayCow(object sender , EventArgs e)
    {
        SwordMan swordMan = sender as SwordMan;
        SearchNextCow(swordMan);
    }

    /// <summary>
    /// �j�M�̪񪺤�
    /// </summary>
    
    private void SearchNextCow(SwordMan swordMan)
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

        swordMan.GetComponent<SwordManAI>().SetCow(cows[cows.IndexOf(returenCow)]);
    }
    

    /// <summary>
    /// �ͦ���
    /// </summary>
    private void SpawnCow()
    {
        if (cows.Count < 5)
        {

            Cow animal = pfList[UnityEngine.Random.Range(0 ,pfList.Count)];

            Cow cow = Cow.CreateCow(transform , animal.transform, new Vector3(UnityEngine.Random.Range(-5f, 5f),UnityEngine.Random.Range(-2f, -5f)));

            cow.OnDestory += Cow_OnDestory;
            cow.OnClick += Cow_OnClick;
            cows.Add(cow);
        }

    }

    // ���Q�ʷ��n������
    private void Cow_OnDestory(object sender, EventArgs e)
    {
        Cow cow = sender as Cow;
        cows.Remove(cow);
        // SearchNextCow();

    }

    // ���Q�I���n������
    private void Cow_OnClick(object sender, EventArgs e)
    {
        Cow cow = sender as Cow;
        if(selectSwordMan != null)
        {
            selectSwordMan.SetCow(cow);
        }
        
    }

    public static Vector3 GetStoragePosition_Static()
    {
        return instance.storage.position;
    }

}
