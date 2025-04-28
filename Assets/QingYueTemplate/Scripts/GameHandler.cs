using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 要傳給其他類的牛
    [SerializeField] Cow tf;

    // 牛群
    List<Cow> cows;

    IUnit unit;
    
    // 生成時間
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
        SwordMan_OnArrivedStorage(this , EventArgs.Empty); // 這裡先觸發了 所以一開始才有牛牛進tf
        
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

    // 到達倉庫該做的事
    private void SwordMan_OnArrivedStorage(object sender, EventArgs e)
    {
        SearchNextCow();
        unit.TargetLock = true;

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

        tf = cows[cows.IndexOf(returenCow)];
    }


    /// <summary>
    /// 生成牛
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

    // 牛被催毀要做的事
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
