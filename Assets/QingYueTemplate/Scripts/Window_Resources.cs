using System;
using UnityEngine;
using UnityEngine.UI;

public class Window_Resources : MonoBehaviour
{

    Text meatAmount;

    private void Awake()
    {
        meatAmount = transform.Find("MeatAmount").GetComponent<Text>();

        Meat.OnMeatAmountChanged += Meat_OnMeatAmountChanged;
    }


    private void Meat_OnMeatAmountChanged(object sender , EventArgs e)
    {
        meatAmount.text = $"¦× : {Meat.MeatAmount}";
    }




}
