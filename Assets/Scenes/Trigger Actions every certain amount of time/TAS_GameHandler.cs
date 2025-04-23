using Unity.Mathematics;
using UnityEngine;

public class TAS_GameHandler : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer.GetComponent<SpriteRenderer>().color = GetColorFromString("FF000030");

        /*
        Debug.Log(GetStringFromColor(new Color(1, 1, 1, 0.7f) , true));
        Debug.Log(HexToFloatNormalize("FF"));
        Debug.Log(FloatToHexNormalize(1));
        */
    }

    private void Start()
    {
        TickTimeSystem.Create();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            new TAS_Building(Utils.GetMouseWorldPosZeroZ() , 30);
        }
    }

    private int HexToDec(string hex)
    {
        return System.Convert.ToInt32(hex , 16);
    }

    private string DecToHex(int dec)
    {
        return dec.ToString("X2");
    }

    private float HexToFloatNormalize(string dec)
    {
        return HexToDec(dec) / 255f;
    }

    private string FloatToHexNormalize(float value)
    {
        return DecToHex(Mathf.RoundToInt(value * 255));
    }

    private Color GetColorFromString(string dec)
    {
        
        float r = HexToFloatNormalize(dec.Substring(0,2));
        float g = HexToFloatNormalize(dec.Substring(2, 2));
        float b = HexToFloatNormalize(dec.Substring(4, 2));
        float alpha = 1f;

        if(dec.Length >= 8)
        {
            alpha = HexToFloatNormalize(dec.Substring(6, 2));
        }

        return new Color(r, g, b , alpha);
    }

    private string GetStringFromColor(Color color , bool useAlpha = false)
    {

        string r = FloatToHexNormalize(color.r);
        string g = FloatToHexNormalize(color.g);
        string b = FloatToHexNormalize(color.b);

        if (!useAlpha)
        {

            return r + g + b;
        }
        else
        {
            string a = FloatToHexNormalize(color.a);
            return r + g + b + a;
        }
    }
}
