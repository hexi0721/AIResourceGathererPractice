using System;
using UnityEngine;

public class TSS_SwordManAI : MonoBehaviour
{
    private enum State{
        WaitingForNextTask,
        ExecutingTask,
    }


    private TSS_ISwordMan swordMan;
    [SerializeField] private State state;
    private float watingTimer;

    private TSS_TaskSystem taskSystem;

    public void SetUp(TSS_TaskSystem taskSystem)
    {
        this.taskSystem = taskSystem;
    }

    public void Start()
    {
         
        state = State.WaitingForNextTask;

        swordMan = GetComponent<TSS_ISwordMan>();
    }

    private void Update()
    {

        switch (state)
        { 
            case State.WaitingForNextTask:

                watingTimer -= Time.deltaTime;
                if(watingTimer <= 0 )
                {
                    float waitingTimerMax = .25f;
                    watingTimer = waitingTimerMax;

                    RequestNewTask();
                }


                break;

            case State.ExecutingTask:

                break;
        
        }


    }

    private void RequestNewTask()
    {
        TSS_TaskSystem.Task task = taskSystem.RequestTask();

        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            ExeuteTase(task);
        }
    }

    private void ExeuteTase(TSS_TaskSystem.Task task)
    {
        swordMan.MoveTo(task.targetPosition, () =>
        {
            state = State.WaitingForNextTask;
        });
    }
}
