using System;
using UnityEngine;

public interface TSS_ISwordMan
{
    public void MoveTo(Vector3 targetPosition , Action OnArrivedAction = null);
}
