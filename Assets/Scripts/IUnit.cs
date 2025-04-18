using UnityEditor.Tilemaps;
using UnityEngine;

public interface IUnit
{

    bool IsIdle();
    void MoveTo(Vector3 position);

    void PlayAnimation();

}
