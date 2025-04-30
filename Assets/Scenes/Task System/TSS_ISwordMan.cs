using System;
using UnityEngine;

public interface TSS_ISwordMan
{
    void MoveTo(Vector3 targetPosition , Action OnArrivedAction = null);
    void PlayIdle2Animation(Action PlayIdle2Animation = null);
}
