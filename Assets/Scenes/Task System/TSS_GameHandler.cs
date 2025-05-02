using UnityEngine;

public class TSS_GameHandler : MonoBehaviour
{
    
    private TSS_TaskSystem taskSystem;
    [SerializeField] private Transform pf_SwordMan;
    [SerializeField] private Transform pf_Cow;

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
            GameObject gameObject = SpawnCow(Utils.GetMouseWorldPosZeroZ());
            TSS_TaskSystem.Task task = new TSS_TaskSystem.Task.ExcuteSlayAnimation() { transform = gameObject.transform, slayAnimation = () => 
            { 
                
                Destroy(gameObject);
            }};

            taskSystem.AddTask(task);


            /*
            TSS_TaskSystem.Task task = new TSS_TaskSystem.Task.ExecuteIdle2 {  };
            taskSystem.AddTask(task);
            */
        }
    }

    private GameObject SpawnCow(Vector3 position)
    {
        Cow cow = Cow.CreateCow(transform , pf_Cow , position);

        return cow.gameObject;
    }

}
