using UnityEngine;
using System;
using Unity.VisualScripting;

public class Meat
{

    public static event EventHandler OnMeatAmountChanged;

    public static int MeatAmount { get; private set; }

    public static void AddMeat(int amount)
    {
        MeatAmount += amount;

        OnMeatAmountChanged?.Invoke(null, EventArgs.Empty);
    }

    public Meat()
    {
        MeatAmount = 0;
    }
}
