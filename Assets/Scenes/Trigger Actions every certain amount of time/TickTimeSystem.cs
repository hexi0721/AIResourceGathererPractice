using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TickTimeSystem : MonoBehaviour
{

    public static event EventHandler OnTick;

    int tick;
    private const float Tick_Time_MAX = .5f;
    private float tickTime;

    private void Update()
    {
        tickTime += Time.deltaTime;
        if(tickTime >= Tick_Time_MAX )
        {
            tickTime -= Tick_Time_MAX;
            tick++;

            OnTick?.Invoke( this, EventArgs.Empty );
        }
    }



}
