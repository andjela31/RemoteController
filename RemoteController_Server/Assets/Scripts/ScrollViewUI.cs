using UnityEngine;

public class ScrollViewUI : MonoBehaviour
{
    RectTransform rt;
    RectTransform parentRT;

    public float widthPercent = 0.48f;
    public float heightPercent = 0.97f;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        parentRT = transform.parent.GetComponent<RectTransform>(); // 999.6178 724.8976
    }

    void Start()
    {
        ApplySize();
    }

    private void ApplySize()
    {
        float parentWidth = parentRT.rect.width;
        float parentHeight = parentRT.rect.height;

        float w = widthPercent * parentWidth;
        float h = heightPercent * parentHeight;

        rt.sizeDelta = new Vector2(w, h);
    }
}