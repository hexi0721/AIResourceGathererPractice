using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEditor.SceneManagement;

public static class TickTimeSystem
{

    public static event EventHandler OnTick;

    private const float Tick_Time_MAX = .5f;

    private static GameObject tickTimeSystemObject;

    public static void Create()
    {
        if (tickTimeSystemObject == null)
        {
            tickTimeSystemObject = new GameObject("TickTimeSystemObject");
            tickTimeSystemObject.AddComponent<TickTimeSystemObject>();
        }
    }

    private class TickTimeSystemObject: MonoBehaviour
    {

        private float tickTime;
        private void Update()
        {
            tickTime += Time.deltaTime;
            if (tickTime >= Tick_Time_MAX)
            {
                tickTime -= Tick_Time_MAX;
                OnTick?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
