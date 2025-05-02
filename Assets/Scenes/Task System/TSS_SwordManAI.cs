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

            if(task is TSS_TaskSystem.Task.MoveToPosition)
            {
                ExeuteTask_MoveToPosition(task as TSS_TaskSystem.Task.MoveToPosition);
            }
            else if (task is TSS_TaskSystem.Task.ExecuteIdle2)
            {
                ExeuteTask_Idle2(task as TSS_TaskSystem.Task.ExecuteIdle2);
            }
            else if (task is TSS_TaskSystem.Task.ExcuteSlayAnimation)
            { 
                ExeuteTask_Slay(task as TSS_TaskSystem.Task.ExcuteSlayAnimation);
            }

        }
    }

    private void ExeuteTask_MoveToPosition(TSS_TaskSystem.Task.MoveToPosition task)
    {
        swordMan.MoveTo(task.targetPosition, () =>
        {
            state = State.WaitingForNextTask;
        });
    }

    private void ExeuteTask_Idle2(TSS_TaskSystem.Task.ExecuteIdle2 task)
    {
        swordMan.PlayIdle2Animation(() =>
        {
            state = State.WaitingForNextTask;
        });
    }

    private void ExeuteTask_Slay(TSS_TaskSystem.Task.ExcuteSlayAnimation task)
    {
        swordMan.MoveTo(task.transform.position , () =>
        {
            swordMan.PlaySlayAnimation(task.transform, () => {

                task.slayAnimation();
                state = State.WaitingForNextTask;
            }
            );

        });


        
    }
}
