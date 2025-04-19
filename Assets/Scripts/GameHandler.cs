using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    static GameHandler instance;

    [Header("牛")]
    [SerializeField] Transform pf_Cow;
    [SerializeField] Transform pf_Calf;
    List<Transform> pfList;

    [Header("倉庫")]
    [SerializeField] Transform storage;

    [Header("主角")]
    [SerializeField] SwordMan swordMan;

    // 要傳給其他類的牛
    Transform tf;

    // 牛群
    [SerializeField] List<Transform> cows;

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
    /// 生成牛
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

    // 牛被催毀要做的事
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
