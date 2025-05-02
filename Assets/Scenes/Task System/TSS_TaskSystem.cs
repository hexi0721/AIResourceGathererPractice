using System;
using System.Collections.Generic;
using UnityEngine;

public class TSS_TaskSystem
{
    
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

    public TSS_TaskSystem()
    {
        taskList = new List<Task>();
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


}
