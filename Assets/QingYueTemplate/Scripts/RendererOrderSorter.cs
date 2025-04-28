using UnityEngine;

public class RendererOrderSorter : MonoBehaviour
{

    Renderer render;
    [SerializeField] float basicSortingOrder = 5000;
    [SerializeField] float offset;
    [SerializeField] bool runOnce = true;

    float timerMax = .1f;
    float timer;
    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timerMax;
            render.sortingOrder = (int)(basicSortingOrder - transform.position.y * 10 - offset);

        }

        if (runOnce)
            Destroy(this);
    }


}
