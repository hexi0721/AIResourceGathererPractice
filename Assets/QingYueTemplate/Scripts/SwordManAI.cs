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
    
    Vector3 cowPostion;
    Vector3 storagePostion;

    TextMesh textMesh;

    Cow cow;
    private void Start()
    {
        unit = GetComponent<IUnit>();
        state = State.Idle;
        textMesh = transform.Find("InventoryAmount").GetComponent<TextMesh>();
        UpdateTextMesh();
    }

    private void UpdateTextMesh()
    {
        textMesh.text = (unit.UnitStat.背負肉的數量 > 0) ? unit.UnitStat.背負肉的數量.ToString() : "";

    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                
                if (cow != null)
                {
                    state = State.MovingToCow;
                    unit.TargetLock = true;
                }
                
                break;

            case State.MovingToCow:

                // 判斷是否直接在範圍內
                if (Vector3.Distance(transform.position, cow.GetPosition()) <= 1.1f)
                {
                    state = State.PlaySlayAnimation;
                    return;
                }

                // 隨時更新位置 畢竟牛會亂跑
                if (cowPostion != cow.GetPosition() && cow != null)
                {
                    cowPostion = cow.GetPosition();
                }

                unit.MoveTo(cow.GetPosition(), 1f, null);

                break;

            case State.PlaySlayAnimation:
                
                // MoveTo 完全做完的情況下 SwordMan.isMoving == false 所以動畫會在 Idle
                if (unit.IsIdle())
                {
                    // 為了將 cow 變顏色以及使用 Damage 方法

                    unit.PlaySlayAnimation(cow.transform , () =>
                    {
                        // 這裡是動畫完整結束做的事
                        UpdateTextMesh();

                        if (unit.UnitStat.背負肉的數量 >= 3)
                        {
                            state = State.MovingToStorage;
                            return;
                        }

                        // 更新位置
                        if (cowPostion != cow.GetPosition() && cow != null)
                        {
                            state = State.MovingToCow;
                        }
                    });
                }
                break;

            case State.MovingToStorage:
                storagePostion = GameHandler.GetStoragePosition_Static();
                unit.TargetLock = false;
                unit.MoveTo(storagePostion, 1f, () =>
                {
                    Meat.AddMeat(unit.UnitStat.背負肉的數量);
                    unit.UnitStat.背負肉的數量 = 0;
                    UpdateTextMesh();
                    
                    state = State.Idle;
                });
                break;
        }
    }

    public void SetCow(Cow cow)
    {
        this.cow = cow;
    }
}
