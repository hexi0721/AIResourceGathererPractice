using UnityEngine;

public class SwordManAI : MonoBehaviour
{
    enum State
    {
        Idle,
        MovingToCow,
        PlaySlayAnimation,
        MovingToStorage
    }

    IUnit unit;
    [SerializeField] State state;
    [SerializeField] int Amount;
    Vector3 cowPostion;
    Vector3 storagePostion;

    private void Start()
    {
        unit = GetComponent<IUnit>();
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                cowPostion = GameHandler.GetCowPosition_Static().position;
                state = State.MovingToCow;
                break;

            case State.MovingToCow:

                // 判斷是否直接在範圍內
                if (Vector3.Distance(transform.position, cowPostion) <= 1.1f)
                {
                    state = State.PlaySlayAnimation;
                    return;
                }

                // 隨時更新位置 畢竟牛會亂跑
                if (cowPostion != GameHandler.GetCowPosition_Static().position)
                {
                    cowPostion = GameHandler.GetCowPosition_Static().position;
                }

                unit.MoveTo(cowPostion, 1f, null);

                break;

            case State.PlaySlayAnimation:
                
                // MoveTo 完全做完的情況下 SwordMan.isMoving == false 所以動畫會在 Idle
                if (unit.IsIdle())
                {
                    // 為了將 cow 變顏色以及使用 Damage 方法
                    Cow cow = GameHandler.GetCowPosition_Static().GetComponent<Cow>();
                    unit.PlaySlayAnimation(cow.transform , () =>
                    {
                        // 這裡是動畫完整結束做的事
                        //cow.Damage(2);
                        
                        Amount += 1;
                        if (Amount >= 3)
                        {
                            state = State.MovingToStorage;
                            return;
                        }

                        // 更新位置
                        if (cowPostion != GameHandler.GetCowPosition_Static().position)
                        {
                            cowPostion = GameHandler.GetCowPosition_Static().position;
                            state = State.MovingToCow;
                        }
                    });
                }
                break;

            case State.MovingToStorage:
                storagePostion = GameHandler.GetStoragePosition_Static();
                unit.MoveTo(storagePostion, 1f, () =>
                {
                    Amount = 0;
                    state = State.Idle;
                });
                break;
        }
    }
}
