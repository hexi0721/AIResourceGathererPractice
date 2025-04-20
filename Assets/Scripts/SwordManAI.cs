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

                // �P�_�O�_�����b�d��
                if (Vector3.Distance(transform.position, cowPostion) <= 1.1f)
                {
                    state = State.PlaySlayAnimation;
                    return;
                }

                // �H�ɧ�s��m �������|�ö]
                if (cowPostion != GameHandler.GetCowPosition_Static().position)
                {
                    cowPostion = GameHandler.GetCowPosition_Static().position;
                }

                unit.MoveTo(cowPostion, 1f, null);

                break;

            case State.PlaySlayAnimation:
                
                // MoveTo �������������p�U SwordMan.isMoving == false �ҥH�ʵe�|�b Idle
                if (unit.IsIdle())
                {
                    // ���F�N cow ���C��H�Ψϥ� Damage ��k
                    Cow cow = GameHandler.GetCowPosition_Static().GetComponent<Cow>();
                    unit.PlaySlayAnimation(cow.transform , () =>
                    {
                        // �o�̬O�ʵe���㵲��������
                        //cow.Damage(2);
                        
                        Amount += 1;
                        if (Amount >= 3)
                        {
                            state = State.MovingToStorage;
                            return;
                        }

                        // ��s��m
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
