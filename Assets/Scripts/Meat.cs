using UnityEngine;
using System;
using Unity.VisualScripting;

public class Meat
{

    public static event EventHandler OnMeatAmountChanged;

    private static int meatAmount;

    public static void AddMeat(int amount)
    {
        meatAmount += amount;

        OnMeatAmountChanged?.Invoke(null, EventArgs.Empty);
    }

    public static int GetMeat()
    {
        return meatAmount;
    }

}
