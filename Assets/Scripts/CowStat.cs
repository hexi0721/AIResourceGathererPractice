using UnityEngine;

[CreateAssetMenu(fileName = "CowStat", menuName = "Scriptable Objects/CowStat")]
public class CowStat : ScriptableObject
{
    Transform TF;
    public int Hp { get; set; }

    public void SetUp(Transform TF , int Hp)
    {
        this.TF = TF;
        this.Hp = Hp;
    }

    public void Damage(int value)
    {
        Hp -= value;
        if (Hp <= 0)
        {
            // Destroy(TF.gameObject);
        }
    }
}
