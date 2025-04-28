using System;
using UnityEngine;

public interface IUnit
{
    class Stat {
        public int ATK;
        public int �I�t�ת��ƶq;
    }

    Stat UnitStat { get; }

    bool TargetLock { get; set; }
    bool IsIdle();
    void MoveTo(Vector3 stopPosition , float distance , Action OnArrivedAtPosition);

    void PlaySlayAnimation(Transform LookAt , Action onAnimationCompleted);

}
