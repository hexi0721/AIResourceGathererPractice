using System;
using Unity.VisualScripting;
using UnityEngine;

public class TAS_Building
{

    private GameObject gameObject;
    private Transform childBar;

    int tick;
    int tickMax;


    public TAS_Building(Vector3 position , int tickMax)
    {

        gameObject = new GameObject("building" , typeof(SpriteRenderer));
        gameObject.AddComponent<RendererOrderSorter>();
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("building1");
        gameObject.transform.position = position;

        childBar = WorldBar_Create(gameObject.transform, new Vector3(-1.5f, -2f), new Vector3(3f, .25f), Color.gray, Color.yellow);


        tick = 0;
        this.tickMax = tickMax;
        TickTimeSystem.OnTick += TickTimeSystem_OnTick;
    }

    private void TickTimeSystem_OnTick(object sender, EventArgs e)
    {
        
        tick++;
        float tickNormalize = tick * 1f / tickMax;
        
        if (tick < tickMax)
        {
            if(tickNormalize >= .5f) gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("building2");
            childBar.localScale = new Vector3(tickNormalize , childBar.localScale.y);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("building3");
            TickTimeSystem.OnTick -= TickTimeSystem_OnTick;
            childBar.parent.gameObject.SetActive(false);
        }
    }

    public Transform WorldBar_Create(Transform parent, Vector3 localPosition, Vector3 localScale, Color? backgroundColor, Color barColor)
    {
        GameObject barObj = new GameObject("World_Bar", typeof(SpriteRenderer));
        barObj.transform.SetParent(parent, false);
        barObj.transform.localPosition = localPosition;
        barObj.transform.localScale = localScale;
        barObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar");
        barObj.GetComponent<SpriteRenderer>().color = (Color)backgroundColor;
        barObj.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Bar");

        GameObject barChildObj = new GameObject("World_BarChild", typeof(SpriteRenderer));
        barChildObj.transform.SetParent(barObj.transform);
        barChildObj.transform.localPosition = Vector3.zero;
        barChildObj.transform.localScale = new Vector3(0, 1);
        barChildObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar");
        barChildObj.GetComponent<SpriteRenderer>().color = (Color)barColor;
        barChildObj.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Bar");
        barChildObj.GetComponent<SpriteRenderer>().sortingOrder = 1;

        return barChildObj.transform;
    }
}
