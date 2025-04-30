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
        textMesh.text = (unit.UnitStat.�I�t�ת��ƶq > 0) ? unit.UnitStat.�I�t�ת��ƶq.ToString() : "";

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

                // �P�_�O�_�����b�d��
                if (Vector3.Distance(transform.position, cow.GetPosition()) <= 1.1f)
                {
                    state = State.PlaySlayAnimation;
                    return;
                }

                // �H�ɧ�s��m �������|�ö]
                if (cowPostion != cow.GetPosition() && cow != null)
                {
                    cowPostion = cow.GetPosition();
                }

                unit.MoveTo(cow.GetPosition(), 1f, null);

                break;

            case State.PlaySlayAnimation:
                
                // MoveTo �������������p�U SwordMan.isMoving == false �ҥH�ʵe�|�b Idle
                if (unit.IsIdle())
                {
                    // ���F�N cow ���C��H�Ψϥ� Damage ��k

                    unit.PlaySlayAnimation(cow.transform , () =>
                    {
                        // �o�̬O�ʵe���㵲��������
                        UpdateTextMesh();

                        if (unit.UnitStat.�I�t�ת��ƶq >= 3)
                        {
                            state = State.MovingToStorage;
                            return;
                        }

                        // ��s��m
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
                    Meat.AddMeat(unit.UnitStat.�I�t�ת��ƶq);
                    unit.UnitStat.�I�t�ת��ƶq = 0;
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
