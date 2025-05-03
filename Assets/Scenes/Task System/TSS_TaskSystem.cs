using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TSS_TaskSystem
{
    
    public class QueueTask
    {

        private Func<Task> tryGetTaskFunc;

        public QueueTask(Func<Task> tryGetTaskFunc)
        {
            this.tryGetTaskFunc = tryGetTaskFunc;
        }

        public Task TryDeQueueTask()
        {
            return tryGetTaskFunc();
        }


    }

    public abstract class Task
    {

        public  class MoveToPosition : Task
        {
            public Vector3 targetPosition;
        }

        public class ExecuteIdle2 : Task
        {
            
        }

        public class ExcuteSlayAnimation : Task
        {
            public Transform transform;
            public Action slayAnimation;
        }
    } 


    private List<Task> taskList;
    private List<QueueTask> queueTasks;

    public TSS_TaskSystem()
    {
        taskList = new List<Task>();
        queueTasks = new List<QueueTask>();

        
    }

    public Task RequestTask()
    {
        if (taskList.Count > 0)
        {
            Task task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }
        else
        {
            return null;
        }
    }

    public void AddTask(Task task)
    {

        taskList.Add(task);

    }

    public void EnQueueTask(QueueTask queueTask)
    {
        queueTasks.Add(queueTask);
    }

    public void EnQueueTask(Func<Task> tryGetTaskFunc)
    {
        QueueTask queueTask = new QueueTask(tryGetTaskFunc);

        queueTasks.Add(queueTask);
    }


    public void DeQueueTask()
    {
        if(queueTasks.Count > 0)
        {
            for (int i = 0; i < queueTasks.Count; i++)
            {
                QueueTask queueTask = queueTasks[i];
                Task task = queueTask.TryDeQueueTask();
                if (task != null)
                {
                    AddTask(task);
                    queueTasks.RemoveAt(i);
                    i--;
                }
            }
        }
    }

}
