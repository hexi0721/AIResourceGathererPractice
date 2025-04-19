using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public interface IUnit
{
    bool TargetLock { get; set; }
    bool IsIdle();
    void MoveTo(Vector3 stopPosition , float distance , Action OnArrivedAtPosition);

    void PlaySlayAnimation(Transform LookAt , Action onAnimationCompleted);

}
