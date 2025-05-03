using System.Collections;
using UnityEngine;

public class TSS_GameHandler : MonoBehaviour
{
    
    private TSS_TaskSystem taskSystem;
    [SerializeField] private Transform pf_SwordMan;
    [SerializeField] private Transform pf_Cow;

    private void Awake()
    {
        taskSystem = new TSS_TaskSystem();
        StartCoroutine(DeQueueTask(taskSystem));



        TSS_SwordMan swordMan = TSS_SwordMan.Create(pf_SwordMan, Vector3.zero);
        TSS_SwordManAI swordManAI = swordMan.GetComponent<TSS_SwordManAI>();
        swordManAI.SetUp(taskSystem);
        
        swordMan = TSS_SwordMan.Create(pf_SwordMan, Vector3.zero);
        swordManAI = swordMan.GetComponent<TSS_SwordManAI>();
        swordManAI.SetUp(taskSystem);
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TSS_TaskSystem.Task task = new TSS_TaskSystem.Task.MoveToPosition { targetPosition = Utils.GetMouseWorldPosZeroZ() };
            taskSystem.AddTask(task);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject gameObject = SpawnCow(Utils.GetMouseWorldPosZeroZ());
            


            float slayTime = Time.time + 5f;
            taskSystem.EnQueueTask(() =>
            {

                if (Time.time > slayTime)
                {
                    TSS_TaskSystem.Task task = new TSS_TaskSystem.Task.ExcuteSlayAnimation()
                    {
                        transform = gameObject.transform,
                        slayAnimation = () =>
                        {
                            Destroy(gameObject);
                        }
                    };

                    return task;

                }
                else
                {
                    return null;
                }
                


            });



        }
    }

    private GameObject SpawnCow(Vector3 position)
    {
        Cow cow = Cow.CreateCow(transform , pf_Cow , position);

        return cow.gameObject;
    }

    private IEnumerator DeQueueTask(TSS_TaskSystem taskSystem)
    {
        while (true)
        {
             
            taskSystem.DeQueueTask();

            yield return new WaitForSeconds(.2f);
        }
    }

}
