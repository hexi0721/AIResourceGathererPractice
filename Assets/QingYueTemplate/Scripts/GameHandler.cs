using System;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour
{
    static GameHandler instance;

    [Header("牛")]
    [SerializeField] Cow pf_Cow;
    [SerializeField] Cow pf_Calf;
    List<Cow> pfList;

    [Header("倉庫")]
    [SerializeField] Transform storage;

    [Header("主角")]
    [SerializeField] SwordMan swordMan;

    // 牛群
    List<Cow> cows;

    // 生成時間
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
        

        swordMan.OnArrivedSotrage += SwordMan_OnArrivedStorage;
        SearchNextCow(); // 這裡先觸發了 所以一開始才有牛牛進tf
        

        
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

    // 到達倉庫該做的事
    private void SwordMan_OnArrivedStorage(object sender, EventArgs e)
    {
        SearchNextCow();

    }

    /// <summary>
    /// 搜尋最近的牛
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

        swordMan.GetComponent<SwordManAI>().SetCow(cows[cows.IndexOf(returenCow)]);
    }


    /// <summary>
    /// 生成牛
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

    // 牛被催毀要做的事
    private void Cow_OnDestory(object sender, EventArgs e)
    {
        Cow cow = sender as Cow;
        cows.Remove(cow);
        SearchNextCow();

    }

    // 牛被點擊要做的事
    private void Cow_OnClick(object sender, EventArgs e)
    {
        Cow cow = sender as Cow;
        swordMan.GetComponent<SwordManAI>().SetCow(cow);
    }

    public static Vector3 GetStoragePosition_Static()
    {
        return instance.storage.position;
    }

}
