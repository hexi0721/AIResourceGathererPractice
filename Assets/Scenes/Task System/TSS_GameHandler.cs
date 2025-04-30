using UnityEngine;

public class TSS_GameHandler : MonoBehaviour
{
    
    private TSS_TaskSystem taskSystem;
    [SerializeField] private Transform pf_SwordMan;

    private void Awake()
    {
        taskSystem = new TSS_TaskSystem();
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
            TSS_TaskSystem.Task task = new TSS_TaskSystem.Task.ExecuteIdle2 {  };
            taskSystem.AddTask(task);
        }
    }

}
